using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    GameObject playerObj;
    public float baseSpeed;
    public float accCoef;
    private WaterSpriteController[] childrenConts;
    
    void Start()
    {
        playerObj = GameObject.Find("Player");
        childrenConts = new WaterSpriteController[transform.childCount];
        for (int i=0; i<transform.childCount; i++)
            childrenConts[i] = transform.GetChild(i).GetComponent<WaterSpriteController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0f, baseSpeed * (Time.timeSinceLevelLoad));
        foreach (WaterSpriteController w in childrenConts)
            w.SetDesiredLevel(transform.position.y);
    }
}
