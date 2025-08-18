using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    protected float m_moveSpeed = 2.0f;

    protected int m_life = 10;

    public enum Target
    {
        Left = 0,
        Right = 1,
    }

    public Target m_target = Target.Right;

    public Vector3 m_targetPosition;

    public delegate void VoidDelegate(Fish fish);

    public VoidDelegate OnDeath;

    public static Fish Create(GameObject prefab, Target target, Vector3 pos)
    {
        GameObject go = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        Fish fish = go.AddComponent<Fish>();
        fish.m_target = target;
        return fish;
    }
    
    public void SetDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            GameObject prefab = Resources.Load<GameObject>("explosion");
            var explosion = Instantiate(prefab, this.transform.position, transform.rotation);
            Destroy(explosion, 1.0f);
            // OnDeath(this);
            Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetTarget();
    }

    void SetTarget()
    {
        float rand = Random.value;
        Vector3 scale = this.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (m_target ==Target.Right ? 1 : -1);
        this.transform.localScale = scale;
        float cameraz = Camera.main.transform.position.z;
        m_targetPosition = Camera.main.ViewportToWorldPoint(new Vector3((int)m_target, 1 * rand, -cameraz));
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector3 pos = Vector3.MoveTowards(this.transform.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
        if(Vector3.Distance(pos, m_targetPosition) < 0.1f)
        {
            m_target = m_target == Target.Left ? Target.Right : Target.Left;
            SetTarget();
        }

        this.transform.position = pos;
    }
}
