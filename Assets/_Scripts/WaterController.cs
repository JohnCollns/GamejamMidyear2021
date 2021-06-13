using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    GameObject playerObj;
    public float baseSpeed;
    public float accCoef;
    
    void Start()
    {
        playerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0f, baseSpeed * (Time.time));
    }
}
