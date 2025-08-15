using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    System.Action<TaFangEnemy> onAttack;
    Transform m_target;
    Bounds m_targetCenter;
    public static void Create(Transform target, Vector3 spawnPos, System.Action<TaFangEnemy> onAttack)
    {
        GameObject prefab = Resources.Load<GameObject>("arrow");
        GameObject go = (GameObject)Instantiate(prefab, spawnPos, Quaternion.LookRotation(target.position - spawnPos));
        Projectile arrowmodel = go.AddComponent<Projectile>();
        arrowmodel.m_target = target;
        arrowmodel.m_targetCenter = target.GetComponentInChildren<SkinnedMeshRenderer>().bounds;
        arrowmodel.onAttack = onAttack;
        Destroy(go, 3.0f);
    }

    void Update()
    {
        if (m_target != null)
            this.transform.LookAt(m_targetCenter.center);

        this.transform.Translate(new Vector3(0, 0, 10 * Time.deltaTime));
        if (m_target != null)
        {
            if (Vector3.Distance(this.transform.position, m_targetCenter.center) < 0.5f)
            {
                onAttack(m_target.GetComponent<TaFangEnemy>());
                Destroy(this.gameObject);
            }
        }
    }
}
