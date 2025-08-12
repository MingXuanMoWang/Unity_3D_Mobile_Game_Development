using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaFangGameManager : MonoBehaviour
{
    public static TaFangGameManager Instance;

    public LayerMask m_groundlayer;

    public int m_wave = 1;

    public int m_waveMax = 10;

    public int m_life = 10;

    public int m_point = 30;

    public Text m_txt_wave;
    public Text m_txt_life;
    public Text m_txt_point;
    public Button m_but_try;
    public Button m_but_player1;
    public Button m_but_player2;

    private bool m_isSelectedButton = false;
    public bool m_debug = true; 
    public List<PathNode> m_PathNodes; 
    
    public List<TaFangEnemy> m_EnemyList = new List<TaFangEnemy>();
    private void Awake()
    {
        Instance = this;
    }


    // Use this for initialization
    void Start()
    {
        // 创建UnityAction，在OnButCreateDefenderDown函数中响应按钮按下事件
        UnityAction<BaseEventData> downAction = new UnityAction<BaseEventData>(OnButCreateDefenderDown);
        // 创建UnityAction，在OnButCreateDefenderDown函数中响应按钮抬起事件
        UnityAction<BaseEventData> upAction = new UnityAction<BaseEventData>(OnButCreateDefenderUp);

        // 创建按钮按下事件Entry
        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener(downAction);

        // 创建按钮抬起事件Entry
        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener(upAction);

        SetWave(1);
        m_txt_life.text = string.Format("生命：<color=yellow>{0}</color>", m_life);
        m_txt_point.text = string.Format("铜钱：<color=yellow>{0}</color>", m_point);
        
        m_but_try.onClick.AddListener(delegate()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        // 默认隐藏重新游戏按钮
        m_but_try.gameObject.SetActive(false);
        EventTrigger trigger1 = m_but_player1.gameObject.AddComponent<EventTrigger>();
        trigger1.triggers = new List<EventTrigger.Entry>();
        trigger1.triggers.Add(down);
        trigger1.triggers.Add(up);
        
        
        EventTrigger trigger2 = m_but_player2.gameObject.AddComponent<EventTrigger>();
        trigger2.triggers = new List<EventTrigger.Entry>();
        trigger2.triggers.Add(down);
        trigger2.triggers.Add(up);


        BuildPath();
    }

    // Update is called once per frame
    void Update()
    {
        // 如果选中创建士兵的按钮则取消摄像机操作
        if (m_isSelectedButton)
            return;

        // 鼠标或触屏操作，注意不同平台的Input代码不同
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        bool press = Input.touches.Length > 0 ? true : false;  // 手指是否触屏
        float mx = 0;
        float my = 0;
        if (press)
        {
            if ( Input.GetTouch(0).phase == TouchPhase.Moved)  // 获得手指移动距离
            {
                mx = Input.GetTouch(0).deltaPosition.x * 0.01f;
                my = Input.GetTouch(0).deltaPosition.y * 0.01f;
            }
        }
#else
        bool press = Input.GetMouseButton(0);
        // 获得鼠标移动距离
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");


#endif
        // 移动摄像机
        GameCamera.Inst.Control(press, mx, my);
    }

    // 更新文字控件"波数"
    public void SetWave(int wave)
    {
        m_wave = wave;
        m_txt_wave.text = string.Format("波数:<color=yellow>{0}/{1}</color>", m_wave, m_waveMax);
    }

    // 更新文字控件"生命"
    public void SetDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            m_life = 0;
            m_but_try.gameObject.SetActive(true); //显示重新游戏按钮
        }

        m_txt_life.text = string.Format("生命:<color=yellow>{0}</color>", m_life);
    }

    // 更新文字控件"铜钱"
    public bool SetPoint(int point)
    {
        if (m_point + point < 0) // ͭ如果铜钱数量不够
            return false;
        m_point += point;
        m_txt_point.text = string.Format("铜钱:<color=yellow>{0}</color>", m_point);

        return true;
    }


    // 按下"创建防守单位按钮"
    void OnButCreateDefenderDown(BaseEventData data)
    {
        m_isSelectedButton = true;
    }

    // 抬起 "创建防守单位按钮" 创建防守单位
    void OnButCreateDefenderUp(BaseEventData data)
    {
        GameObject go = data.selectedObject;
        // // 创建射线
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hitinfo;
        // // 检测是否与地面相碰撞
        // if (Physics.Raycast(ray, out hitinfo, 1000, m_groundlayer))
        // {
        //     // 如果选中的是一个可用的格子
        //     if (TileObject.Instance.getDataFromPosition(hitinfo.point.x, hitinfo.point.z) ==
        //         (int)Defender.TileStatus.GUARD)
        //     {
        //         // 获得碰撞点位置
        //         Vector3 hitpos = new Vector3(hitinfo.point.x, 0, hitinfo.point.z);
        //         // 获得Grid Object坐位位置
        //         Vector3 gridPos = TileObject.Instance.transform.position;
        //         // 获得格子大小
        //         float tilesize = TileObject.Instance.tileSize;
        //         // 计算出所点击格子的中心位置
        //         hitpos.x = gridPos.x + (int)((hitpos.x - gridPos.x) / tilesize) * tilesize + tilesize * 0.5f;
        //         hitpos.z = gridPos.z + (int)((hitpos.z - gridPos.z) / tilesize) * tilesize + tilesize * 0.5f;
        //
        //         // 获得选择的按钮GameObject，将简单通过按钮名字判断选择了哪个按钮 
        //         GameObject go = data.selectedObject;
        //
        //         if (go.name.Contains("1")) //如果按钮名字包括“1”
        //         {
        //             if (SetPoint(-15))
        //             {
        //                 
        //             } // 减15个铜钱，然后创建近战防守单位
        //                 // Defender.Create<Defender>(hitpos, new Vector3(0, 180, 0));
        //         }
        //         else if (go.name.Contains("2")) // 如果按钮名字包括“2”
        //         {
        //             if (SetPoint(-20))
        //             {
        //                 
        //             } // 减20个铜钱，然后创建远程防守单位
        //                 // Defender.Create<Archer>(hitpos, new Vector3(0, 180, 0));
        //         }
        //     }
        // }
    
        m_isSelectedButton = false;
    }

    [ContextMenu("BuildPath")]
    void BuildPath()
    {
        m_PathNodes = new List<PathNode>();
        // 通过路点的Tag查找所有的路点
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");
        for (int i = 0; i < objs.Length; i++)
        {
            // m_PathNodes.Add(objs[i].GetComponent<PathNode>());
        }
    }


    void OnDrawGizmos()
    {
        if (!m_debug || m_PathNodes == null)
            return;
    
        Gizmos.color = Color.blue; // 将路点连线的颜色设为蓝色
        foreach (PathNode node in m_PathNodes) // 遍历路点
        {
            if (node.m_next != null)
            {
                // 在路点间画出连接线
                Gizmos.DrawLine(node.transform.position, node.m_next.transform.position);
            }
        }
    }
}