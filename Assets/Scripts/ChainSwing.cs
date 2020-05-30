using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class ChainSwing : MonoBehaviour
{
    [SerializeField] Rigidbody2D body2d;
    [SerializeField] float leftPushRange; // how far the chain should swing on the left
    [SerializeField] float rightPushRange; // how far the chain should swing on the right
    [SerializeField] float velocityThreshold;
    [SerializeField] int maceDamage;

    // Start is called before the first frame update
    void Start()
    {
        //references
        body2d = GetComponent<Rigidbody2D>();
        body2d.angularVelocity = velocityThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        Push();
    }
 
   /// <summary>Pushes the chain </summary>
   public void Push()
    { 
        //checking if it has reached the limit on the right side and if it has enough speed to go further
        if (transform.rotation.z > 0 && transform.rotation.z < rightPushRange && body2d.angularVelocity > 0 && body2d.angularVelocity < velocityThreshold)
        {
            body2d.angularVelocity = velocityThreshold;
        }
        else if (transform.rotation.z < 0 && transform.rotation.z > leftPushRange && body2d.angularVelocity < 0 && body2d.angularVelocity > velocityThreshold * -1)
        {
            body2d.angularVelocity = velocityThreshold * -1;
        }
    }
}
