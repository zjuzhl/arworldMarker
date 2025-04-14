using InsightAR.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProduceARStart_Hand_Flower : MonoBehaviour
{
    public GameObject target = null;
    public GameObject succeed = null;

    public Image handmask;
    public Image handmask1;

    public GameObject tipShowHand;
    public GameObject tipShowClick;
    public GameObject tipReset;

    private bool recognized = false;
    private float lostTimeCount = 10;

    private float screenWidthR;
    private float screenHeightR;
    private float algWidth;
    private float algHeight;
    private bool accWidth = false;

    void Start()
    {
        ProduceStart();
        ShowHandTip();

        algWidth = 640.0f;
        algHeight = 480.0f; // 640*480
        

        string configpath = Application.streamingAssetsPath + "/ezxr/config_hand_i";
#if !UNITY_EDITOR
#if UNITY_ANDROID
        configpath = InsightARUtility.CopyAssetsToApplicationDataDir("ezxr") + "/config_hand_a";
#endif
#endif
        LSGameManager.Instance.InitConfigPath(configpath);
        LSGameManager.Instance.InitARScene();
    }

    private void ProduceStart()
    {
        target.SetActive(false);
        succeed.SetActive(false);
        tipReset.SetActive(false);
    }

    private void CloseSucceedUI()
    {
        succeed.SetActive(false);
    }

    private void ShowHandTip() 
    {
        tipShowHand.SetActive(true);
        tipShowClick.SetActive(false);
    }

    void Update()
    {
        bool istracking = false;
        //Debug.Log("tracking result: " + InsightTracking.GetResultString(1));
        var ges = InsightTracking.GetResultString(1);
        //var ges = "{\"status\":1,\"gesture_num\":1,\"gestures\":[{\"cls_id\":6,\"class_name\":\"palm\",\"score\":0.89898989,\"rect\":\"290.65 251.24 451.49 647.30\"}]}";
        if (string.IsNullOrEmpty(ges)) return;
        gestureResult gesture = JsonUtility.FromJson<gestureResult>(ges);
        if (gesture.status == 1 && gesture.gesture_num > 0) 
        {
            Debug.Log("tracking result class_name " + gesture.gestures[0].class_name + " , " + gesture.gestures[0].rect);

            var rect = gesture.gestures[0].rect;
            var num = rect.Split(new char[] { ' ' });
            if (float.TryParse(num[0], out float x) && 
                float.TryParse(num[1], out float y) && 
                float.TryParse(num[2], out float w) && 
                float.TryParse(num[3], out float h)) 
            {
                var rt = handmask.GetComponent<RectTransform>();

                // fix：兼容折叠屏
                screenWidthR = Screen.width / algWidth;
                screenHeightR = Screen.height / algHeight;
                accWidth = Screen.width * 1.5f < Screen.height * 1.0f;
                if (accWidth)
                {
                    rt.anchoredPosition = new Vector2((x - 190) * screenWidthR, -(y - 30) * screenWidthR);
                    rt.sizeDelta = new Vector2(w * screenWidthR * 1.1f, h * screenWidthR * 1.1f);
                }
                else {
                    rt.anchoredPosition = new Vector2((x - 60) * screenWidthR * 0.5f, -(y - 160) * screenWidthR * 0.5f);
                    rt.sizeDelta = new Vector2(w * screenWidthR * 0.55f, h * screenWidthR * 0.55f);
                }
                var rt1 = handmask1.GetComponent<RectTransform>();
                Vector2 pos = rt.anchoredPosition;
                Vector2 rec = rt.sizeDelta;
                rt1.anchoredPosition = new Vector2(pos.x + rec.x * 0.5f, Screen.height + pos.y - rec.y * 0.5f);

                if (gesture.gestures[0].class_name == "palm")
                {
                    if (target.activeSelf == false)
                    {
                        target.SetActive(true);
                        succeed.SetActive(true);
                        tipShowHand.SetActive(false);
                        Invoke(nameof(CloseSucceedUI), 3);
                    }

                    var tp = Camera.main.ScreenToWorldPoint(new Vector3(rt1.anchoredPosition.x, rt1.anchoredPosition.y, 2.5f));
                    var cur = target.transform.position;
                    target.transform.position = Vector3.MoveTowards(cur, tp, 0.04f);
                    target.transform.LookAt(Camera.main.transform);

                    recognized = true;
                    istracking = true;
                    lostTimeCount = 0;
                }
                else {
                    if (recognized) 
                    {
                        var tp = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 2.5f));
                        var cur = target.transform.position;
                        target.transform.position = Vector3.MoveTowards(cur, tp, 0.04f);
                        target.transform.LookAt(Camera.main.transform);
                    }
                }
            }
        }

        if (istracking)
        {
            if (tipReset.activeSelf)
            {
                tipReset.SetActive(false);
            }
            if (tipShowClick.activeSelf)
            {
                tipShowClick.SetActive(false);
            }
        }
        else {
            if (recognized) 
            {
                // 延迟
                if (lostTimeCount < 1.0f)
                {
                    lostTimeCount += Time.deltaTime;
                }
                else 
                {
                    if (!tipReset.activeSelf)
                    {
                        tipReset.SetActive(true);
                    }
                    if (!tipShowClick.activeSelf)
                    {
                        tipShowClick.SetActive(true);
                    }
                }
            }
        }
    }

    public void resetPose() 
    {
        var tp = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 2.5f));
        //var cur = target.transform.position;
        target.transform.position = tp;
        target.transform.LookAt(Camera.main.transform);
    }

    [System.Serializable]
    public class gestureResult
    {
        public int status;
        public int gesture_num;
        public List<gestureInfo> gestures;
    }

    [System.Serializable]
    public class gestureInfo
    {
        public int cls_id;
        public string class_name;
        public double score;
        public string rect;
    }
}


