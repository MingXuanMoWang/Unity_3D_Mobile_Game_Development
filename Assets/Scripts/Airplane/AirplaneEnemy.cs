using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneEnemy : MonoBehaviour
{
    public float m_speed = 1;

    public float m_life = 10;
    public Transform m_explosionFX;
    protected float m_rotSpeed = 30;

    public int m_point = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    protected virtual void UpdateMove()
    {
        float rx = Mathf.Sin(Time.time) * Time.deltaTime;
        transform.Translate(new Vector3(rx, 0, -m_speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerRocket")
        {
            AirplaneRocket rocket = other.GetComponent<AirplaneRocket>();
            if (rocket != null)
            {
                m_life -= rocket.m_power;
                if (m_life <= 0)
                {
                    AirplaneGameManager.Instance.AddScore(m_point);
                    Instantiate(m_explosionFX, transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
            }
        }else if (other.tag == "Player")
        {
            m_life = 0;
            AirplaneGameManager.Instance.AddScore(m_point);
            Instantiate(m_explosionFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
