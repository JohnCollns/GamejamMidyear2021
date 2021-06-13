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
    private float waterLevel;
    void Start()
    {
        timeCoef = (2 * Mathf.PI) / travelPeriod;
        waterLevel = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Waves go up and down
        curOsc = oscillationAmount * Mathf.Cos(Time.time * timeCoef);
        //print("CurOsc: " + curOsc +" = " + oscillationAmount + " * " + "Cos(" + Time.time + " * " + timeCoef + ") [" + Mathf.Cos(Time.time * timeCoef) + "]");
        //transform.position = new Vector3(transform.position.x + sideSpeed * Time.deltaTime, transform.position.y + curOsc);
        transform.position = new Vector3(transform.position.x + sideSpeed * Time.deltaTime, waterLevel + curOsc);
        if (transform.position.x > teleportThreshold)
            transform.position = new Vector3(transform.position.x + sideSpeed * Time.deltaTime - (4* 20), waterLevel);
        //print("Water: " + name + " beyond threshold: " + (transform.position.x > teleportThreshold));
    }

    public void SetDesiredLevel(float newLevel)
    {
        waterLevel = newLevel;
    }
}
