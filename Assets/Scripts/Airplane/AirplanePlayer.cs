using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AirplanePlayer : MonoBehaviour
{

    public AudioClip m_shootClip;
    protected AudioSource m_audio;
    public Transform m_explosionFX;
    
    public float m_speed;
    public float m_life = 1;

    public Transform m_rocket;

    private float m_rocketTimer = 0;

    protected Vector3 m_targetPos;

    public LayerMask m_inputMask;
    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // float movev = 0;
        // float moveh = 0;
        //
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     movev += m_speed * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     movev -= m_speed * Time.deltaTime;
        // }
        //
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     moveh -= m_speed * Time.deltaTime;
        // }
        //
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     moveh += m_speed * Time.deltaTime;
        // }
        //
        // this.transform.Translate(new Vector3(moveh, 0, movev));
        
        MoveTo();

        m_rocketTimer -= Time.deltaTime;
        if (m_rocketTimer <= 0)
        {
            m_rocketTimer = 0.1f;
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                // Instantiate(m_rocket, transform.position, transform.rotation);
                var p = PathologicalGames.PoolManager.Pools["mypool"];
                p.Spawn("AirplaneRocket", transform.position, transform.rotation, null);
                m_audio.PlayOneShot(m_shootClip);
            } 
        }

    }

    void MoveTo()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 ms = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(ms);
            RaycastHit hitinfo;
            var iscast = Physics.Raycast(ray, out hitinfo, 1000, m_inputMask);
            if (iscast)
            {
                m_targetPos = hitinfo.point;
            }
        }
        Vector3 pos = Vector3.MoveTowards(transform.position, m_targetPos, m_speed * Time.deltaTime);
        this.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag != "PlayerRocket")
        {
            m_life -= 1;
            AirplaneGameManager.Instance.ChangeLife(m_life);
            if (m_life <= 0)
            {
                Debug.Log("死亡");

                Instantiate(m_explosionFX, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
