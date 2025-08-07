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
    
    protected FPSEnemySpawn m_spawn;

    public void Init(FPSEnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }

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
        // 如果主角生命为0，什么也不做
        if (m_player.m_life <= 0)
            return;
        // 更新计时器
        m_timer -= Time.deltaTime;

        // 获取当前动画状态
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

        // 如果处于待机且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle")
            && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);

            // 待机一定时间

            if (m_timer > 0)
                return;

            // 如果距离主角小于1.5米，进入攻击动画状态
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 1.5f)
            {
                // 停止寻路
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);
            }
            else
            {
                // 重置定时器
                m_timer = 1;

                // 设置寻路目标点
                m_agent.SetDestination(m_player.m_transform.position);

                // 进入跑步动画状态
                m_ani.SetBool("run", true);
            }
        }

        // 如果处于跑步且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run")
            && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            // 每隔1秒重新定位主角的位置
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);

                m_timer = 1;
            }

            // 如果距离主角小于1.5米，向主角攻击
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <= 1.5f)
            {
                // 停止寻路
                m_agent.ResetPath();
                // 进入攻击状态
                m_ani.SetBool("attack", true);
            }
        }

        // 如果处于攻击且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack")
            && !m_ani.IsInTransition(0))
        {
            // 面向主角
            RotateTo();
            m_ani.SetBool("attack", false);

            // 如果动画播完，重新进入待机状态
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);

                // 重置计时器待机2秒
                m_timer = 2;
                m_player.OnDamage(1);
            }
        }

        // 如果处于死亡且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.death") &&
            !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("death", false);
            // 当播放完成死亡动画
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_spawn.m_enemyCount--;
                FPSGameManager.Instance.SetScore(100);
                // 销毁自身
                Destroy(this.gameObject);
            }
        }
    }
    
    public void OnDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
            m_agent.ResetPath();
        }
    }
}