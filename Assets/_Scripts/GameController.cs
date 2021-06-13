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

    public GameObject[] proceduralBag;
    private List<int> generationList = new List<int>();
    public float procItemDist = 5.6f;
    private int procItemsMade = 0;
    public float procItemMaxX;
    private float baseHeight;
    void Awake()
    {
        playerCont = playerObj.GetComponent<PlayerController>();
        playerCont.GetGCont(this);
        waterCont = waterObj.GetComponent<WaterController>();
        cam = camObj.GetComponent<Camera>();
        int[] firstGen = GenerateSequence();
        for (int i = 0; i < firstGen.Length; i++)
            generationList.Add(firstGen[i]);
        baseHeight = camObj.transform.position.y + cam.orthographicSize;
        //print("Procedural bag length: " + proceduralBag.Length);
        //string outp = "";
        //for (int i = 0; i < generationList.Count; i++)
        //    outp += generationList[i]; 
        //print("Generation list for i: "+outp);
        //outp = "";
        //foreach (int a in generationList)
        //    outp += a.ToString();
        //print("Generation list foreach: "+outp);
    }

    // Update is called once per frame
    void Update()
    {

        // Procedural generation
        int camHeight = Mathf.FloorToInt(camObj.transform.position.y / procItemDist);
        for (int i=0; i < camHeight - procItemsMade; i++) // Made the difference between these two variables number of procedural items
        {
            procItemsMade++; 
            Vector3 origin = new Vector3(Random.Range(-procItemMaxX, procItemMaxX), baseHeight + procItemDist * procItemsMade);
            int nextObj = GetNextGenObj();
            GameObject newObj = Instantiate(proceduralBag[nextObj], origin, Quaternion.identity);
            print("Procedurally making new item["+ procItemsMade+"]: " + newObj.name + ", (bag item: "+ nextObj+"), at: " + origin);
        }
    }

    private int GetNextGenObj()
    {
        int next = generationList[0];
        generationList.RemoveAt(0);
        if (generationList.Count < 3)
        {
            int[] newGen = GenerateSequence();
            for (int i = 0; i < newGen.Length; i++)
                generationList.Add(i);
        }
        return next;
    }

    private int[] GenerateSequence()
    {
        int[] output = new int[proceduralBag.Length];
        for (int i = 0; i < proceduralBag.Length; i++)
            output[i] = i;
        // Using Fisher-Yates shuffle
        for (int i = 0; i < proceduralBag.Length; i++)
        {
            int j = Random.Range(0, i + 1);
            int temp = output[i];
            output[i] = output[j];
            output[j] = temp;
        }
        //string stringOut = "Sequence: ";
        //foreach (int i in output)
        //{
        //    stringOut += i + ", ";
        //}
        //print(stringOut);
        return output;
    }
}
