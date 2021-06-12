using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject camObj;
    private Camera cam;
    public GameObject playerObj;
    private PlayerController playerCont;
    public GameObject waterObj;
    private WaterController waterCont;
    void Awake()
    {
        playerCont = playerObj.GetComponent<PlayerController>();
        playerCont.GetGCont(this);
        waterCont = waterObj.GetComponent<WaterController>();
        cam = camObj.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
