using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPSGameManager : MonoBehaviour
{
    public static FPSGameManager Instance;
    public int m_score = 0;

    public static int m_hiscore = 0;

    public int m_ammo = 100;

    private FPSPlayer m_player;

    public Text txt_ammo;
    public Text txt_score;
    public Text txt_hisscore;
    public Text txt_life;
    public Button button_restart;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSPlayer>();
        button_restart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        txt_hisscore.text = "Hiscore " + m_hiscore.ToString();
        button_restart.gameObject.SetActive(false);
    }
    public void SetScore(int score)
    {
        m_score += score;
        txt_score.text = "Score " + m_score.ToString();
        if (m_score > m_hiscore)
        {
            m_hiscore = m_score;
            txt_hisscore.text = "Hiscore " + m_hiscore.ToString();
        }
    }
    
    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;
        if (m_ammo <= 0)
        {
            m_ammo = 100 - m_ammo;
        }
        txt_ammo.text = m_ammo.ToString() + "/100";
    }
    
    public void SetLife(int life)
    {
        txt_life.text = life.ToString();
        if (life <= 0)
        {
            button_restart.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
