using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FPSEnemy : MonoBehaviour
{
    Transform m_transform;

    private Animator m_ani;
    NavMeshAgent m_agent;
    
    FPSPlayer m_player;

    private float m_moveSpeed = 2.5f;
    private float m_rotSpeed = 5.0f;
    private float m_timer = 2;
    private float m_life = 15;
    
    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_ani = this.GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSPlayer>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_moveSpeed;
        m_agent.SetDestination(m_player.transform.position);
    }

    void RotateTo()
    {
        Vector3 targetdir = m_player.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
