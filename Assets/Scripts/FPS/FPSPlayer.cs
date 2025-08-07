using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayer : MonoBehaviour
{
    public Transform m_transform { get; set; }
    public Transform m_camTransform { get; set; }
    private Vector3 m_camRot;
    float m_camHeight = 1.4f;

    private CharacterController m_ch;

    private float m_moveSpeed = 3.0f;

    private float m_gravity = 2.0f;

    public Transform m_muzzlepoint;
    public LayerMask m_layer;
    public Transform m_fx;
    public AudioClip m_audio;
    private float m_shootTimer = 0;

    public int m_life = 5;
    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        m_camTransform = Camera.main.transform;
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;
        
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_life <= 0)
        {
            return;
        }
        Control();

        m_shootTimer -= Time.deltaTime;
        if(Input.GetMouseButton(0) && m_shootTimer <= 0)
        {
            m_shootTimer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);
            FPSGameManager.Instance.SetAmmo(1);
            RaycastHit info;
            bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
            if (hit)
            {
                Debug.Log("击中目标：" + info.transform.tag);
                if(info.transform.tag.CompareTo("Enemy") == 0)
                {
                    FPSEnemy enemy = info.transform.GetComponent<FPSEnemy>();
                    if (enemy != null)
                    {
                        enemy.OnDamage(1);
                    }
                }

                var fx = Instantiate(m_fx, info.point, info.transform.rotation);
                Destroy(fx.gameObject, 1.0f);
            }
        }
    }

    void Control()
    {
        Vector3 motion = Vector3.zero;
        motion.x = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        motion.z = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        motion.y -= m_gravity * Time.deltaTime;
        m_ch.Move(m_transform.TransformDirection(motion));
        
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");
        
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;
        
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot;
    
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);
    }
    
    public void OnDamage(int damage)
    {
        m_life -= damage;
        FPSGameManager.Instance.SetLife(m_life);
        if (m_life <= 0)
        {
            Screen.lockCursor = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Spawn.tif", true);
    }
}
