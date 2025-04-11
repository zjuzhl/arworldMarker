using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using InsightAR.Internal;
using UnityEngine.SceneManagement;

public class ProduceUI :MonoBehaviour
{
    public Button btnQuitAR;

    // Start is called before the first frame update
    void Start()
    {
        
        btnQuitAR.onClick.AddListener(()=> {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
