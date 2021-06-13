using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float belowThresh;
    public float upMove;
    private Transform playerTran;
    void Start()
    {
        playerTran = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y + belowThresh < Camera.main.transform.position.y)
            transform.Translate(0f, upMove, 0f);
    }
}
