using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    int mode = 0;
    public string mainSceneName = "SampleScene";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            mode++;
            switch (mode)
            {
                case 1:
                    transform.GetChild(0).gameObject.active = false;
                    break;
                case 2:
                    //transform.GetChild(1).gameObject.active = false;
                    SceneManager.LoadScene(mainSceneName);
                    break;
                case 3:
                    SceneManager.LoadScene(mainSceneName);
                    break;

            }
        }
    }
}
