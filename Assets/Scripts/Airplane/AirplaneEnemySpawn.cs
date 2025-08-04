using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirplaneEnemySpawn : MonoBehaviour
{
    public Transform m_enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));
        Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));
            Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
