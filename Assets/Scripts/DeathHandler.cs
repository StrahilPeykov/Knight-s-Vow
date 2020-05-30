using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<Boss>().isAlive==false)
        {         
            Destroy(this.GetComponent<Boss>());
            Destroy(this);        
        }
    }
}
