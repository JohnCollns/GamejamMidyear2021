using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerObj;
    private Rigidbody2D rb;
    private Transform lowerBound, upperBound;
    void Start()
    {
        lowerBound = transform.GetChild(0);
        upperBound = transform.GetChild(1);
        rb = playerObj.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerObj.position.y >= upperBound.position.y) // Player is above upper bound, move camera with their upwards speed * 1
        //{
        //    transform.position += new Vector3(0f, rb.velocity.y * Time.deltaTime, 0f);
        //}
        //else if (playerObj.position.y > lowerBound.position.y) // Player is between the upper and lower bound, move with their speed times how close they are to upper bound
        //{
        //    // (current - minimum) / (maximum - minimum)
        //    float proportion = (playerObj.position.y - lowerBound.position.y) / (upperBound.position.y - lowerBound.position.y);
        //    transform.position += new Vector3(0f, rb.velocity.y * Time.deltaTime * proportion, 0f);
        //}
        if (playerObj.position.y > lowerBound.position.y && rb.velocity.y > 0)
        {
            float proportion = Mathf.Clamp01((playerObj.position.y - lowerBound.position.y) / (upperBound.position.y - lowerBound.position.y));
            transform.position += new Vector3(0f, rb.velocity.y * Time.deltaTime * proportion, 0f);
        }
    }
}
