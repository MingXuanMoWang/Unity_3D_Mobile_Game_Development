using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSEnemySpawn : MonoBehaviour
{
    public Transform m_enemy;
    
    public int m_enemyCount;

    public int m_maxEnemy = 3;

    public float m_timer = 0;
    protected Transform m_transform;
    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_enemyCount >= m_maxEnemy)
        {
            return;
        }

        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            m_timer = Random.value * 15.0f;
            if(m_timer < 5.0f)
            {
                m_timer = 5.0f;
            }

            Transform obj = (Transform)Instantiate(m_enemy, m_transform.position, Quaternion.identity);
            var enemy = obj.GetComponent<FPSEnemy>();
            enemy.Init(this);
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "item.png", true);
        }
    }
}
