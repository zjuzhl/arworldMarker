using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InsightAR.Internal;
public class LSAlgDebugInfo : MonoBehaviour
{

    private int bLastTrackingStatus = -100;
    private int bLastCloudLocStatus = -100;
    public Text tTrackingText;
    public Text tCloudLocationText;


    private int bLastScreenWidth = -100;
    private int bLastScreenHeight = -100;
    public Text tScreenSizeText;
    public Text tAppPauseStateText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetTrackingStatusVisible();
        SetCloudLocStatusVisible();
        SetScreenSizeDisplay();
    }

    private void SetTrackingStatusVisible() {
        var status = InsightTracking.status;
        if (tTrackingText && status != bLastTrackingStatus)
        {
            tTrackingText.text = "tracking status: " + status.ToString();
        }
    }

    private void SetCloudLocStatusVisible()
    {
        var status = InsightTracking.cloudLocationStatus;
        if (tCloudLocationText && status != bLastCloudLocStatus)
        {
            tCloudLocationText.text = "cloudLocation status: " + status.ToString();
        }
    }

    private void SetScreenSizeDisplay() {
        if (bLastScreenWidth != Screen.width || bLastScreenHeight != Screen.height) {
            bLastScreenWidth = Screen.width;
            bLastScreenHeight = Screen.height;
            var t = "ScreenSize: w " + Screen.width.ToString() + ", h " + Screen.height.ToString();
            tScreenSizeText.text = t;
            //Debug.Log(t);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        tAppPauseStateText.text = "AppPauseState: is " + pause.ToString();
    }
}
