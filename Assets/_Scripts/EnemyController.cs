using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float slowAmount, slowDuration;
    public float agroRadius;
    private int angry = 0; //   0-passive,  1-angry
    public float[] speed;
    public float travelPeriod;
    private float timeCoef;
    public float[] angleRange; //0-min ,    1-max
    private float angle;
    public float turningCoef = 0.3f;
    private Vector3 passiveWalk;

    private Rigidbody2D rb;
    private BoxCollider2D box;
    private CircleCollider2D cir;
    private bool ballMode = false;
    private bool dangerousBall = false;

    public Vector3[] push;
    public float[] pushMag;

    //private Rigidbody2D playerrb;
    private Transform playerTran;
    private PlayerController pCont;
    private int platLayer;
    private int playerLayer;
    void Start()
    {
        angle = Random.Range(angleRange[0] * Mathf.Deg2Rad, angleRange[1] * Mathf.Deg2Rad);
        int dir = transform.position.x < 0 ? 1 : -1; // On the right: -1,    on the left: 1
        passiveWalk = new Vector3(Mathf.Cos(angle) * dir, Mathf.Sin(angle), 0f) * speed[0];
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        cir = GetComponent<CircleCollider2D>();
        box.enabled = false;
        timeCoef = (2 * Mathf.PI) / travelPeriod;

        //platLayer = LayerMask.GetMask("Platform");
        //playerLayer = LayerMask.GetMask("Player");
        platLayer = 6;
        playerLayer = 10;

        playerTran = GameObject.Find("Player").transform;
        pCont = playerTran.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        int dir = transform.position.x < 0 ? 1 : -1;
        //print("Spider: " + name + ", dir: " + dir + ", at x: " + transform.position.x);
        Debug.DrawLine(transform.position, new Vector3(Mathf.Cos(angleRange[0] * Mathf.Deg2Rad) * dir, Mathf.Sin(angleRange[0] * Mathf.Deg2Rad)) * 5f + transform.position);
        Debug.DrawLine(transform.position, new Vector3(Mathf.Cos(angleRange[1] * Mathf.Deg2Rad) * dir, Mathf.Sin(angleRange[1] * Mathf.Deg2Rad)) * 5f + transform.position);
        // Red line: passive walk
        Debug.DrawLine(transform.position, passiveWalk / speed[0] * 5f + transform.position, Color.red);
        // Blue line: angle direction
        Debug.DrawLine(transform.position, new Vector3(Mathf.Cos(angle * dir), Mathf.Sin(angle)) * 5f + transform.position, Color.blue);

        //int dir = transform.position.x < 0 ? 1 : -1;
        //Debug.DrawLine(transform.position, new Vector3(Mathf.Cos(angleRange[0] * Mathf.Deg2Rad * dir), Mathf.Sin(angleRange[0] * Mathf.Deg2Rad * dir)) * 5f + transform.position);
        //Debug.DrawLine(transform.position, new Vector3(Mathf.Cos(angleRange[1] * Mathf.Deg2Rad * dir), Mathf.Sin(angleRange[1] * Mathf.Deg2Rad * dir)) * 5f + transform.position);
        //// Red line: passive walk
        //Debug.DrawLine(transform.position, passiveWalk / speed[0] * 5f + transform.position, Color.red);
        //// Blue line: angle direction
        //Debug.DrawLine(transform.position, new Vector3(Mathf.Cos(angle * dir), Mathf.Sin(angle * dir)) * 5f + transform.position, Color.blue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("Enemy has collided with: " + collision.gameObject.name + ", on layer: " + collision.gameObject.layer);
        //print("Platlayer: " + platLayer + ", player layer: " + playerLayer);
        if (collision.gameObject.layer == platLayer)// Bounce off. 
        {
            if (!ballMode)
            {
                passiveWalk = new Vector3(passiveWalk.x, -passiveWalk.y);
                //rb.velocity = passiveWalk * Mathf.Clamp01(Mathf.Cos(Time.time * timeCoef));
                rb.velocity = passiveWalk * Mathf.Clamp01(Mathf.Cos(Time.time * timeCoef) + 1);
                //print("Enemy: " + name + " collided with wall, changing direction");
            }
            else
            {
                dangerousBall = true;
            }
        }
        if (collision.gameObject.layer == 11 && !ballMode) // hit a wall
        {
            passiveWalk = new Vector3(-passiveWalk.x, passiveWalk.y);
            rb.velocity = passiveWalk * Mathf.Clamp01(Mathf.Cos(Time.time * timeCoef) + 1);
        }
        if (collision.gameObject.layer == playerLayer)  // Either hurt the player or be hurt
        {
            if (!ballMode)
            {
                //if (collision.GetContact(0).point.y > (transform.position.y + (1/3) * cir.bounds.max.y))
                if (collision.transform.GetChild(0).position.y > (transform.position.y + (1/3) * cir.bounds.extents.y)) // player landed above the mid point, take damage
                {
                    Hurt();
                }
                else // Hurt the player
                {
                    Vector3 pushVec = (collision.transform.position - transform.position).normalized * pushMag[0];
                    collision.rigidbody.AddForce(pushVec, ForceMode2D.Impulse);
                    rb.AddForce(pushVec * -(2 / 3), ForceMode2D.Impulse);
                    print("Hurting the player, pushing them by vector: " + pushVec);
                    //pCont.GetSlowed()
                    //print("Applying to rigidbody: " + collision.rigidbody.gameObject.name + ", or other rigidbody: " + collision.otherRigidbody.name);
                    //collision.otherRigidbody.AddForce(push[0], ForceMode2D.Impulse);
                    //rb.AddForce(push[0] * -(2 / 3), ForceMode2D.Impulse);
                }
            }
            else if (dangerousBall) // Hurt the player even more
            {
                Vector3 pushVec = (transform.position - collision.transform.position).normalized * pushMag[1];
                collision.otherRigidbody.AddForce(pushVec, ForceMode2D.Impulse);
                rb.AddForce(pushVec * -(2 / 3), ForceMode2D.Impulse);
            }
            //print("Enemy: " + name + " collided with player");
        }
        if (collision.gameObject.layer == 9)
            Hurt();
    }

    private void FixedUpdate()
    {
        if (!ballMode)
        {
            if (Vector3.Distance(playerTran.position, transform.position) <= agroRadius) // If the spider is close enough change to agro
            {
                angry = 1;
            }
            if (angry == 0) // Non angro, just travelling
                rb.velocity = passiveWalk * Mathf.Clamp01(Mathf.Cos(Time.time * timeCoef) + 1);
            else// Aggrod, turn towards player and move in that direction
            {
                // Following from John's Procedural Animation research project. 
                //Vector3 targetDir = (playerTran.position - transform.position).normalized;
                //float angleFromForward = Vector3.SignedAngle(transform.forward, targetDir, Vector3.forward);
                //Quaternion curQuat = transform.rotation;
                //transform.Rotate(0f, angleFromForward, 0f); 
                //Quaternion targetQuat = transform.rotation;
                //transform.rotation = Quaternion.Lerp(curQuat, targetQuat, turningCoef);
                //rb.velocity = transform.forward * speed[1] * Mathf.Clamp01(Mathf.Cos(Time.time * timeCoef));
                ////print("Spider ")
                Vector3 targetDir = (playerTran.position - transform.position).normalized;
                rb.velocity = Vector3.RotateTowards(rb.velocity.normalized, targetDir, turningCoef, 1) * speed[1] * Mathf.Clamp01(Mathf.Cos(Time.time * timeCoef));
            }

        }
        
    }

    private void Hurt()
    {
        ballMode = true;
        rb.gravityScale = 1;
        box.enabled = true;
    }


}
