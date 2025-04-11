using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{

    public Button btnEnterARWorld;
    public Button btnEnter2DTracker;
    public Button btnEnterHand;
    // Start is called before the first frame update
    void Start()
    {
        btnEnterARWorld.onClick.AddListener(()=> {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        });

        btnEnter2DTracker.onClick.AddListener(() => {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        });

        btnEnterHand.onClick.AddListener(() => {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
