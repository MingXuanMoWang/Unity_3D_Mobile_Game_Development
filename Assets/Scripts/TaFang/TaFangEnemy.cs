using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaFangEnemy : MonoBehaviour
{
    public PathNode m_currentNode;

    public int m_life = 15;

    public int m_maxlife = 15;

    public float m_speed = 2;
    private Transform m_lifebarObj;
    Slider m_lifebar;
    public System.Action<TaFangEnemy> onDeath;
    // Start is called before the first frame update
    void Start()
    {
        TaFangGameManager.Instance.m_EnemyList.Add(this);

        GameObject prefab = (GameObject)Resources.Load("Canvas3D");
        m_lifebarObj = ((GameObject)Instantiate(prefab, Vector3.zero, Camera.main.transform.rotation, this.transform)).transform;
        m_lifebarObj.localPosition = new Vector3(0, 2.0f, 0);
        m_lifebarObj.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        m_lifebar = m_lifebarObj.GetComponentInChildren<Slider>();
        StartCoroutine(UpdateLifebar());
    }
    
    IEnumerator UpdateLifebar()
    {
        m_lifebar.value = (float)m_life / (float)m_maxlife;
        m_lifebarObj.transform.eulerAngles = Camera.main.transform.eulerAngles;
        yield return 0;
        StartCoroutine(UpdateLifebar());
    }

    // Update is called once per frame
    void Update()
    {
        RotateTo();
        MoveTo();
    }

    public void RotateTo()
    {
        var position = m_currentNode.transform.position - transform.position;
        position.y = 0;
        var targetRotation = Quaternion.LookRotation(position);
        float next =
            Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, 120 * Time.deltaTime);
        this.transform.eulerAngles = new Vector3(0, next, 0);
    }
    public void MoveTo()
    {
        Vector3 pos1 = this.transform.position;
        Vector3 pos2 = m_currentNode.transform.position;
        float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
        if (dist < 1.0f)
        {
            if (m_currentNode.m_next == null)
            {
                TaFangGameManager.Instance.SetDamage(1);
                DestroyMe();
            }
            else
            {
                m_currentNode = m_currentNode.m_next;
            }
        }
        this.transform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));
    }
    
    public void SetDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            TaFangGameManager.Instance.SetPoint(5);
            DestroyMe();
        }
    }

    public void DestroyMe()
    {
        TaFangGameManager.Instance.m_EnemyList.Remove(this);
        onDeath(this);
        Destroy(this.gameObject);
    }
}
