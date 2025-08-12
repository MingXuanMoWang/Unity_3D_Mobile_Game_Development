using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaFangEnemySpawner : MonoBehaviour
{
    public PathNode m_startNode; // 起始节点
    private int m_liveEnemy = 0;  // 存活的敌人数量
    public List<WaveData> waves; // 战斗波数配置数组
    int enemyIndex = 0; //生成敌人数组的下标
    int waveIndex = 0; //战斗波数数组的下标

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnEnemies()); // 开始生成敌人

    }

    IEnumerator SpawnEnemies()
    {
        Debug.Log("开始生成敌人");

        // yield return new WaitForEndOfFrame();
  
        TaFangGameManager.Instance.SetWave((waveIndex + 1));

        WaveData wave = waves[waveIndex];
        yield return new WaitForSeconds(wave.interval);

        while (enemyIndex < wave.enemyPrefab.Count)
        {
            Vector3 dir = m_startNode.transform.position - this.transform.position; // 初始方向
            GameObject enmeyObj = (GameObject)Instantiate(wave.enemyPrefab[enemyIndex], transform.position, Quaternion.LookRotation(dir)); // 创建敌人
            TaFangEnemy enemy = enmeyObj.GetComponent<TaFangEnemy>();  // 获得敌人的脚本
            enemy.m_currentNode = m_startNode; // 设置敌人的第一个路点

            // 设置敌人数值，这里只是简单示范
            // 数值配置适合放到一个专用的数据库（SQLite数据库或JSON、XML格式的配置）中读取
            enemy.m_life = wave.level * 3;  
            enemy.m_maxlife = enemy.m_life;

            m_liveEnemy++; // 增加敌人数量
            enemy.onDeath= new System.Action<TaFangEnemy>((TaFangEnemy e) =>{ m_liveEnemy--; });// 当敌人死掉时减少敌人数量

            enemyIndex++;  // 更新敌人数组下标
            yield return new WaitForSeconds(wave.interval); // 生成敌人时间间隔
        }
        // 创建完全部敌人，等待敌人全部被消灭
        while(m_liveEnemy>0)
        {
            yield return 0;
        }

        enemyIndex = 0; // 重置敌人数组下标
        waveIndex++;  // 更新战斗波数
        if (waveIndex< waves.Count) // 如果不是最后一波
        {
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            // 通知胜利
        }
    }

    // 在编辑器中显示一个图标
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawner.tif");
    }
}
