using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpell : MonoBehaviour
{
    [SerializeField] float fireRate = 3f;
    public Transform firePoint;
    public GameObject spellBallPrefab;
    private Boss boss;
    [SerializeField] bool isBossAlive;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        if (boss != null ) 
        {
           InvokeRepeating("Shoot", 2f, fireRate);
        }
    }

    private void Update()
    {
        isBossAlive= boss.isAlive;
    }

    ///<summary>Shooting logic - spawns a bullet on firepoint location</summary> 
    public void Shoot()
    {
        if (boss == null)
        {
            Destroy(gameObject);
        }
        if (boss.isAlive && !boss.isHalfHealth)
        {
            Instantiate(spellBallPrefab, firePoint.position, firePoint.rotation); // spawns a ball on the firepoint location with its rotation
        }
        else if (!boss.isThirdHealth || boss.isHalfHealth)
        {
            
        }
        else if (boss.isAlive==false)
        {
            Destroy(gameObject);
        }
    }

}
