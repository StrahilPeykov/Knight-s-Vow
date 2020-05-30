using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFix : MonoBehaviour
{
    public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;
    public float speed = 1;
    float myWidth;
    float myHeight;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        myHeight = mySprite.bounds.extents.y;
        myWidth = mySprite.bounds.extents.x; // = how wide our sprite is
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check to see if there's ground in front of us before moving forward with linecasting
        Vector2 lineCastPosition = myTrans.position - myTrans.right * myWidth  ; // finds our position and goes to our left by our width
        bool isGrounded = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down, enemyMask);
        bool isBlocked = Physics2D.Linecast(lineCastPosition, lineCastPosition - new Vector2(myTrans.right.x, myTrans.right.y), enemyMask);

        //If there's no ground, turn around or if he is blocked by another object
        if (!isGrounded || isBlocked)
        {
            Vector3 currRotation = myTrans.eulerAngles;
            currRotation.y += 180;
            myTrans.eulerAngles = currRotation;

        }
        //always move forward
        Vector2 myVel = myBody.velocity;
        myVel.x = -myTrans.right.x * speed;
        myBody.velocity = myVel;
    }
}
