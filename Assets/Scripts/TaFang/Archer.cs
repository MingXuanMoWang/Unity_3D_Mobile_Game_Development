using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Defender {

    protected override void Init()
    {
 
        m_attackArea = 5.0f;
        m_power = 1;
        m_attackInterval = 1.0f;
 
        CreateModel("archer");
        StartCoroutine(Attack()); 

    }

    protected override IEnumerator Attack()
    {
        while (m_targetEnemy == null || !m_isFackEnemy) 
            yield return 0;
        m_ani.CrossFade("attack", 0.1f); 

        while (!m_ani.GetCurrentAnimatorStateInfo(0).IsName("attack")) 
            yield return 0;
        float ani_lenght = m_ani.GetCurrentAnimatorStateInfo(0).length; 
        yield return new WaitForSeconds(ani_lenght * 0.5f); 
        if (m_targetEnemy != null) 
        {
            Vector3 pos = this.m_model.transform.Find("atkpoint").position;
            
            Projectile.Create(m_targetEnemy.transform, pos, (TaFangEnemy enemy) =>
            {
                enemy.SetDamage(m_power);
                m_targetEnemy = null;
            });
        }
        yield return new WaitForSeconds(ani_lenght * 0.5f); 
        m_ani.CrossFade("idle", 0.1f);

        yield return new WaitForSeconds(m_attackInterval); 

        StartCoroutine(Attack()); 
    }
}
