using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject weight;
    private Rigidbody2D wrb;
    
    private GameController gCont;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private float halfColHeight;

    // Get a sprite renderer
    private bool facingRight = true;

    public float allowableGroundedDist = 0.005f;
    private bool isGrounded = false;
    private int platformLayerMask;
    
    // For the following 0- indicates grounded,     1- indicates airborne
    public float[] jumpAcc;
    private Vector3[] jumpForce;
    public float[] maxSpeed;
    public float[] sidewaysAcc;
    private Vector3[] sideForce;
    public float wrongWayMultiplier = 1.5f;
    public float maxFuel;
    private float curFuel;
    public float fuelRechargeRate;  // How many fuel units per second the fuel recharges on the ground. 
    public float fuelDrainRate;     // How many fuel units per second the fuel depletes when using the jetpack. 
    private bool jumpCommand;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        wrb = weight.GetComponent<Rigidbody2D>();
        platformLayerMask = (LayerMask.GetMask("Platform") | LayerMask.GetMask("Weight"));

        print("Player pos: " + transform.position + ", col center: " + col.bounds.center +
            ", col extents: " + col.bounds.extents + ", col max: " + col.bounds.max + ", col min: " + col.bounds.min);

        curFuel = maxFuel;
        jumpForce = new Vector3[2]{new Vector3(0f, jumpAcc[0], 0f), new Vector3(0f, jumpAcc[1], 0f)};
        sideForce = new Vector3[2] { new Vector3(sidewaysAcc[0], 0f, 0f), new Vector3(sidewaysAcc[1], 0f, 0f) };
        print("Jump Force: " + jumpForce[0] + ", " + jumpForce[1]);
    }

    private void FixedUpdate()
    {
        isGrounded = GetGrounded();
        int controlIndex = isGrounded ? 0 : 1; //   0- grounded,    1- airborne

        if (jumpCommand)
        {
            if (isGrounded)
                rb.AddForce(jumpForce[0], ForceMode2D.Impulse);
            else
            {
                if (curFuel > 0)
                {
                    rb.AddForce(jumpForce[1]);
                    curFuel = Mathf.Clamp(curFuel - fuelDrainRate * Time.fixedDeltaTime, 0, maxFuel);
                }
            }
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            float multiplier = AccelerateAwayFromVelo() ? wrongWayMultiplier : 1f;
            if (Mathf.Abs(rb.velocity.x) < maxSpeed[controlIndex] || AccelerateAwayFromVelo())
                rb.AddForce(sideForce[controlIndex] * Input.GetAxisRaw("Horizontal") * multiplier);
        }

        if (isGrounded && !jumpCommand) // Player is on the ground and not going to jump, so recharge jetpack. 
            curFuel = Mathf.Clamp(curFuel + fuelRechargeRate * Time.fixedDeltaTime, 0, maxFuel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Jump") > 0f)
            jumpCommand = true;
        else
            jumpCommand = false;
        //print("Jump command: " + jumpCommand + ", jump axis: " + Input.GetAxisRaw("Jump"));
    }

    private bool GetGrounded()
    {
        //Vector3 origin = transform.position;
        float minDist = Mathf.Infinity;
        Vector3 originLeft = col.bounds.min;
        Vector3 originRight = col.bounds.min + new Vector3(col.bounds.extents.x * 2, 0f,0f);
        //print("Grounded raycast starting from left: " + originLeft + ", right: " + originRight);
        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector3.down, 5f, platformLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector3.down, 5f, platformLayerMask);
        if (hitLeft.collider != null)
            minDist = hitLeft.distance;
        if (hitRight.collider != null)
        {
            //print("Hit right hit: " + hitRight.collider.name + ", with distance away: " + hitRight.distance + ", result isGrounded: " + (hitRight.distance < minDist));
            if (hitRight.distance < minDist)
                minDist = hitRight.distance;
        }
        return minDist <= allowableGroundedDist;
    }

    private bool AccelerateAwayFromVelo()
    {
        return (rb.velocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0) || // Player is moving right and input is move left
                (rb.velocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0);  // Player is moving left and input is move right
    }

    //IEnumerator Recharge()
    //{

    //}

    public void GetGCont(GameController newGCont)
    {
        this.gCont = newGCont;
    }
}
