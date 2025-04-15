using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFlower : MonoBehaviour
{
    public Transform colliderTarget;
    public Transform colliderTarget1;

    public Transform createTarget;
    public Transform createTarget1;

    private float clickedTime = 0;
    private Transform selectedTarget;


    // Start is called before the first frame update
    void Start()
    {
        createTarget.gameObject.SetActive(false);
        createTarget1.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            var state = touch.phase;
            if (state == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, 1 << LayerMask.NameToLayer("Flower")))
                {
                    if (hitInfo.transform == colliderTarget)
                    {
                        clickedTime = Time.time;
                        selectedTarget = createTarget;
                        colliderTarget.gameObject.SetActive(false);
                    }
                    if (hitInfo.transform == colliderTarget1)
                    {
                        clickedTime = Time.time;
                        selectedTarget = createTarget1;
                        colliderTarget1.gameObject.SetActive(false);
                    }
                }
            }
            else if (state == TouchPhase.Moved)
            {

            }
            else if (state == TouchPhase.Ended)
            {
                if (clickedTime > 0 && Time.time - clickedTime < 0.4f) 
                {
                    if (selectedTarget != null) 
                    {
                        selectedTarget.gameObject.SetActive(true);
                        selectedTarget = null;
                        clickedTime = 0;
                    }
                }
            }
        }
    }
}
