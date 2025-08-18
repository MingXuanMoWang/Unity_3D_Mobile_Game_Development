using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private float m_moveSpeed = 10.0f;

    public static Fire Create(Vector3 pos, Vector3 angle)
    {
        GameObject prefab = Resources.Load<GameObject>("fire");
        var fireSprite = Instantiate(prefab, pos, Quaternion.Euler(angle));
        var f = fireSprite.AddComponent<Fire>();
        Destroy(fireSprite, 2.0f);
        return f;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Fish f = other.GetComponent<Fish>();
        if (f == null)
        {
            return;
        }
        else
        {
            f.SetDamage(1);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0, m_moveSpeed * Time.deltaTime, 0));

    }
}
