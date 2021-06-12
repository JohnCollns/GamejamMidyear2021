using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpriteController : MonoBehaviour
{
    public float sideSpeed;
    public float teleportThreshold = 30f;
    public float travelPeriod;
    private float timeCoef;
    public float oscillationAmount = 2f;
    public float curOsc = 0f;
    void Start()
    {
        timeCoef = (2 * Mathf.PI) / travelPeriod;
    }

    // Update is called once per frame
    void Update()
    {
        // Waves go up and down
        curOsc = oscillationAmount * Mathf.Cos(Time.time * timeCoef);
        print("CurOsc: " + curOsc +" = " + oscillationAmount + " * " + "Cos(" + Time.time + " * " + timeCoef + ") [" + Mathf.Cos(Time.time * timeCoef) + "]");
        transform.position = new Vector3(transform.position.x + sideSpeed * Time.deltaTime, transform.position.y + curOsc);
        if (transform.position.x > teleportThreshold)
            transform.position = new Vector3(transform.position.x + sideSpeed * Time.deltaTime - (3* 20), transform.position.y);
    }
}
