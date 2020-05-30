using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enrage : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] float fireRate = 3f;
    public GameObject triggerTile;
    public GameObject enrageBall;
    private float positionOnX;
    public bool isEnraged;
    
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        if (boss!=null)
        {
            InvokeRepeating("spawnTile", 1f, fireRate);
        }
    }

    // Update is called once per frame
    public void spawnTile()
    {    
        GameObject tile = triggerTile;
        if (boss.isAlive && boss.isHalfHealth)
        {
            isEnraged = true;
            positionOnX = Random.Range(107f, 120f);
            Instantiate(tile, new Vector2(positionOnX, 50.5f), boss.transform.rotation);
            StartCoroutine(WaitBeforeSpawningBall());
        }
        IEnumerator  WaitBeforeSpawningBall()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(enrageBall, new Vector2(positionOnX, 52f), boss.transform.rotation);
        }

        if (boss.isThirdHealth)
        {
            Destroy(this);
        }
    }
}
