using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    #region ohter clients offense value
    //player state value
    public bool ISLOCKON { get; set; }
    public bool ISDEAD { get; set; }
    
    //player invasion value
    public bool WANTINVASION { get; set; }
    public int KILLCOUNT { get; set; }
    public int DESTROYPLANTCOUNT { get; set; }
    public int DESTROYBUILDINGCOUNT { get; set; }
    #endregion

    #region master clients defense value
    //master clients value
    public bool INVASIONALLOW { get; set; }
    public int TOTALDRAGONCOUNT { get; set; }
    public int TOTALSEEDCOUNT { get; set; }
    public int TOTALBUILDINGCOUNT { get; set; }
    public int TOTALCOIN { get; set;}
    #endregion

    #region Invasion Game Controll Value
    public bool ISDEFENSE { get; set; }
    public float GAMETIME { get; set; }
    public int STEALCOIN { get; set; }
    public bool ISTIMEOVER { get; set; }
    #endregion

    Coroutine timerCoroutine = null;

    DeffenseBattleUIManager deffenseBattleUIManager;
    DefenseUIManager deffenseUIManager;

    private void Awake()
    {
        // Defense UI <-> Defense Battle UI
        deffenseUIManager = GameObject.Find("DefenseUIManager").GetComponent<DefenseUIManager>();
        deffenseBattleUIManager = GameObject.Find("DeffenseBattleUIManager").GetComponent<DeffenseBattleUIManager>();

        deffenseUIManager.gameObject.SetActive(true);
        deffenseBattleUIManager.gameObject.SetActive(false);
    }

    void Start()
    {
        Initializing();
    }

    private void Initializing() //init function
    {
        //player state
        GameManager.INSTANCE.ISDEAD = false;
        GameManager.INSTANCE.ISLOCKON = false;

        //player invasion value
        GameManager.INSTANCE.WANTINVASION = false;
        GameManager.INSTANCE.KILLCOUNT = 0;
        GameManager.INSTANCE.DESTROYPLANTCOUNT = 0;
        GameManager.INSTANCE.DESTROYBUILDINGCOUNT = 0;

        //master clients value
        GameManager.INSTANCE.INVASIONALLOW = false;
        GameManager.INSTANCE.TOTALDRAGONCOUNT = 0;
        GameManager.INSTANCE.TOTALSEEDCOUNT = 0;
        GameManager.INSTANCE.TOTALBUILDINGCOUNT = 0;
        GameManager.INSTANCE.TOTALCOIN = 0;

        //Invasion Game Controll Value
        GameManager.INSTANCE.ISDEFENSE = true;
        GameManager.INSTANCE.GAMETIME = 0;
        GameManager.INSTANCE.STEALCOIN = 0;
        GameManager.INSTANCE.ISTIMEOVER = false;

        
    }

    private void Update()
    {
        DefenseSceneChange();
    }

    #region Time Controll
    public void TimerStart()
    {
        timerCoroutine = StartCoroutine(TimeCount());
    }

    public void TimeOut()
    {
        StopCoroutine(timerCoroutine);
    }
    #endregion 

    void DefenseSceneChange()//Defense Scene Toggle
    {
        

        // 어떤 키를 눌렀을 때 DefenseUI 를 비활성화
        if (Input.GetKeyDown(KeyCode.D))
        {
            // DefenseBattleUI 를 활성화
            deffenseUIManager.gameObject.SetActive(false);
            deffenseBattleUIManager.gameObject.SetActive(true);

            GameObject[] attackSkillObjects = GameObject.FindGameObjectsWithTag("AttackSkill");

            for (int i = 0; i < attackSkillObjects.Length; i++)
            {
                ObjectPoolingManager.inst.Destroy(attackSkillObjects[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 게임 시간 초기화
            GameManager.INSTANCE.GAMETIME = 0f;
            // 게임 오버일때 다시 DefenseUI 활성화, DefenseBattleUI 비활성화
            deffenseUIManager.gameObject.SetActive(true);
            deffenseBattleUIManager.gameObject.SetActive(false);

        }

    }

    IEnumerator TimeCount() //invasion timer
    {
        while (true)
        {
            GameManager.INSTANCE.GAMETIME += Time.deltaTime;

            if (GameManager.INSTANCE.GAMETIME > 60f)
            {
                CoinRavish();
                Time.timeScale = 0f;
                GameManager.INSTANCE.ISTIMEOVER = true;
                yield break;
            }

            yield return null;
        }
    }

    public void CoinRavish()//coin ravish calculation
    {
        float stealCoinKillScale = 0f;
        float stealCoinSaboScale = 0f;
        float killCoinPoint = 0f;
        float saboCoinPoint = 0f;

        //KillCoin scale calculation
        {
            float numerator =   ((float)GameManager.INSTANCE.KILLCOUNT + (float)GameManager.INSTANCE.DESTROYPLANTCOUNT);
            float denominator = ((float)GameManager.INSTANCE.TOTALDRAGONCOUNT + (float)GameManager.INSTANCE.TOTALSEEDCOUNT);
            stealCoinKillScale = numerator / denominator;
        }

        //SaboCoin scale calculation
        {
            float numerator =   ((float)GameManager.INSTANCE.DESTROYBUILDINGCOUNT);
            float denominator = ((float)GameManager.INSTANCE.TOTALBUILDINGCOUNT);
            stealCoinSaboScale = numerator / denominator;
        }

        //kill coin calculation
        {
            float numerator =   ((float)GameManager.INSTANCE.TOTALCOIN * stealCoinKillScale);
            float denominator = (20f);
            killCoinPoint =  numerator / denominator;
        }

        //sabo coin calculation
        {
            float numerator = ((float)GameManager.INSTANCE.TOTALCOIN * stealCoinSaboScale);
            float denominator = (20f);
            saboCoinPoint = numerator / denominator;
        }

        //real steal coin calculation
        GameManager.INSTANCE.STEALCOIN = (int)(killCoinPoint + saboCoinPoint);
    }
}
