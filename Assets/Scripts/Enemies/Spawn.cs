using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int DifficultyCost;
    public GameObject[] Enemies;
    public float Delay;
    public float LocationVariance;

    void Awake()
    {
        foreach(GameObject Enemy in Enemies)
        {
            Enemy.SetActive(false);
        }
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        bool AllKilled = true;
        foreach(GameObject Enemy in Enemies)
        {
            if (Enemy != null)
            {
                AllKilled = false;
                break;
            }
        }
        if (AllKilled)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpawnEnemies()
    {
        foreach(GameObject Enemy in Enemies)
        {
            yield return new WaitForSeconds(Delay);
            Enemy.SetActive(true);
            Enemy.GetComponent<Damageable>().SpawnAnim();
            Enemy.transform.parent = null;

            float RandX = Enemy.transform.position.x + Random.Range(-LocationVariance, LocationVariance);
            float RandY = Enemy.transform.position.y + Random.Range(-LocationVariance, LocationVariance);
            Enemy.transform.position = new Vector2(RandX, RandY);
        }
    }
}
