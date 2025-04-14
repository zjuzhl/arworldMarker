using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    public Transform colliderTarget;
    public Transform rotateTarget;
    public float rotationSpeed = 1f;
    public bool xrotEnable = false;
    public bool yrotEnable = false;
    private bool isDragging = false;
    private Quaternion prequa;
    private Vector3 prepos;

    // scale
    private float curTouchesDis = 0;
    public Transform scaleTarget;
    private Vector3 prescale;
    private float curScaleSize = 1.0f;
    private float maxScaleSize = 2;
    private float minScaleSize = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        prepos = rotateTarget.localPosition;
        prequa = rotateTarget.localRotation;

        prescale = scaleTarget.localScale;
    }

    public void resetPose() 
    {
        rotateTarget.localPosition = prepos;
        rotateTarget.localRotation = prequa;
    }

    public void resetScale()
    {
        scaleTarget.localScale = prescale;
        curScaleSize = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1) 
        {
            if (!colliderTarget || !rotateTarget) return; 

            var touch = Input.GetTouch(0);
            var state = touch.phase;
            if (state == TouchPhase.Began) 
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000)) 
                {
                    if (hitInfo.transform == colliderTarget) 
                    {
                        isDragging = true;
                    }
                }
            }
            else if (state == TouchPhase.Moved)
            {
                if (isDragging && rotateTarget != null) 
                {
                    var delta = touch.deltaPosition;                    
                    if (xrotEnable) {
                        rotateTarget.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime);
                    }
                    if (yrotEnable) {
                        rotateTarget.Rotate(Vector3.left, delta.y * rotationSpeed * Time.deltaTime);
                    }
                }
            }
            else if (state == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }

        if (Input.touchCount == 2) 
        {
            if (!scaleTarget) return; // 没有可缩放的对象

            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);
            var dis = Vector2.Distance(touch1.position, touch0.position);
            
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                curTouchesDis = dis;// 当触摸开始时，记录初始距离
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) 
            {
                if (curTouchesDis <= 0) return;
                var l = dis - curTouchesDis;
                if (l > 0)
                {
                    // larger
                    curScaleSize += (l * 0.1f);
                    if (curScaleSize > maxScaleSize) curScaleSize = maxScaleSize;
                    scaleTarget.localScale = prescale * curScaleSize;
                }
                if (l < 0)
                {
                    // smaller
                    curScaleSize -= (l * 0.1f);
                    if (curScaleSize < minScaleSize) curScaleSize = minScaleSize;
                    scaleTarget.localScale = prescale * curScaleSize;
                }
                curTouchesDis = dis;
            }
            if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended) 
            {
                curTouchesDis = 0;
            }
        }
    }
}
