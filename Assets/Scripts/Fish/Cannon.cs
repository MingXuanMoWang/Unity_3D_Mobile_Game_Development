using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private float m_shootTimer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        m_shootTimer -= Time.deltaTime;
        Vector3 ms = Input.mousePosition;
        ms = Camera.main.ScreenToWorldPoint(ms);
        Vector3 mypos = this.transform.position;
        if (Input.GetMouseButton(0))
        {
            Vector2 targetDir = ms - mypos;
            float angle = Vector2.Angle(targetDir, Vector2.up);
            if (ms.x > mypos.x)
            {
                angle = -angle;
            }
            this.transform.eulerAngles = new Vector3(0, 0, angle);
            if (m_shootTimer <= 0)
            {
                m_shootTimer = 0.1f;
                Fire.Create(this.transform.TransformPoint(0, 1, 0), new Vector3(0, 0, angle));
            }
        }
    }
}
