using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;


public class OffenseUIManager : MonoBehaviour
{

    //camera components
    GameObject cameraArm = null;
    Vector3 initCamDir = Vector3.zero;

    //Hp UI components
    PlayerController player = null;
    Canvas playerUI = null;
    Slider hpSlider = null;
    Slider hpFollowSlider = null;

    //Dragon Hp UI components
    GameObject dragonHpGroupObj = null;
    Slider dragonHpSlider = null;
    Slider dragonHpFollowSlider = null;
    List<Dragon> fruitragons = null;

    //Cine Producton components
    RectTransform cineUp = null;
    RectTransform cineDown = null;

    //variables value
    public float ADDVALUE { get; set; }

    Coroutine curCoroutine = null;

    //constant value
    Vector3 initUp;
    Vector3 initDown;


    //OffenseEndUI
    GameObject offenseEndUI = null;

    // 게임매니저에 있는 변수 사용, 계산해서 텍스트로 띄우기
    int DragonKilledCountNum = 0;
    int PlantKilledCountNum = 0;
    int BuildingDestroyCountNum = 0;

    // 파괴한 오브젝트 텍스트로 받아오기
    TextMeshProUGUI DragonKilledCount;
    TextMeshProUGUI PlantKilledCount;
    TextMeshProUGUI BuildingDestroyCount;
    // 총 점수 계산
    TextMeshProUGUI TotalScore;
    TextMeshProUGUI GetCoin;
    // 랭크
    TextMeshProUGUI RANK;




    private void Awake()
    {
        //camera components
        cameraArm = GameObject.Find("CameraArm");

        //hp ui components
        player = FindObjectOfType<PlayerController>();
        playerUI = GameObject.Find("PlayerUI").GetComponent<Canvas>();
        hpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        hpFollowSlider = GameObject.Find("HpFollowSlider").GetComponent<Slider>();

        //dragon hp ui components
        fruitragons = new List<Dragon>();
        dragonHpGroupObj = GameObject.Find("DragonHp");
        dragonHpSlider = GameObject.Find("DragonHpSlider").GetComponent<Slider>();
        dragonHpFollowSlider = GameObject.Find("DragonHpFollowSlider").GetComponent<Slider>();

        //cine production components
        cineUp = GameObject.Find("CineViewUp").GetComponent<RectTransform>();
        cineDown = GameObject.Find("CineViewDown").GetComponent<RectTransform>();

        // Offense End UI Panel Found in Canvas
        offenseEndUI = GameObject.Find("OffenseEndUI");

        DragonKilledCount       =GameObject.Find("DragonsKilled_number").GetComponent<TextMeshProUGUI>();
        PlantKilledCount        =GameObject.Find("PlantKilled_number").GetComponent<TextMeshProUGUI>();
        BuildingDestroyCount    =GameObject.Find("DestroyBuilding_number").GetComponent<TextMeshProUGUI>();
        TotalScore              =GameObject.Find("TotalScore_number").GetComponent<TextMeshProUGUI>();
        GetCoin                 =GameObject.Find("GetCoins_number").GetComponent<TextMeshProUGUI>();
        RANK                    =GameObject.Find("Rank").GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {

        // Offense End UI
        offenseEndUI.SetActive(false);

        // 
        initCamDir = cameraArm.transform.forward;

        //player hp delegate
        player.playerEvent.callBackPlayerHPChangeEvent += OnChangedHp;

        //dragon hp delegate
        for (int i = 0; i < FindObjectsOfType<Dragon>().Length; i++)
        {
            fruitragons.Add(FindObjectsOfType<Dragon>()[i]);
            fruitragons[i].dragonEvent.callBackDragonHPChangeEvent += OnChangeDragonHP;
        }

        //dragon hp view init
        dragonHpGroupObj.SetActive(false);

        GameManager.INSTANCE.ISLOCKON = false;
        ADDVALUE = 200f;

        initUp = cineUp.position;
        initDown = cineDown.position;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerUIControll();
        CineProductControll();
        GameEndControll();
    }


    private void PlayerUIControll()
    {
        if (GameManager.INSTANCE.ISLOCKON)
        {
            playerUI.transform.localRotation = Quaternion.LookRotation(initCamDir);
            //ui방향 고정 시키도록 하기
        }
        else
        {
            playerUI.transform.LookAt(Camera.main.transform);
        }
    }
    private void CineProductControll()
    {
        if (GameManager.INSTANCE.ISDEAD || GameManager.INSTANCE.ISTIMEOVER)
        {
            curCoroutine = StartCoroutine(ProductionOff());
        }


        if (GameManager.INSTANCE.ISLOCKON)
        {
            curCoroutine = StartCoroutine(ProductionOn());
        }
        else if (!GameManager.INSTANCE.ISLOCKON)
        {
            curCoroutine = StartCoroutine(ProductionOff());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DefenseScene");
            asyncOperation.allowSceneActivation = true;
        }
    }
    IEnumerator ProductionOn()
    {
        float timer = 0f;

        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }

        while (true)
        {
            timer += Time.deltaTime;

            cineDown.position = Vector3.Lerp(cineDown.position, initDown + Vector3.up * ADDVALUE, 0.05f);
            cineUp.position = Vector3.Lerp(cineUp.position, initUp + Vector3.down * ADDVALUE, 0.05f);

            yield return null;
            if (timer > 0.8f) yield break;
        }
    }
    IEnumerator ProductionOff()
    {
        float timer = 0f;

        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }

        while (true)
        {
            timer += Time.deltaTime;

            cineDown.position = Vector3.Lerp(cineDown.position, initDown, 0.05f);
            cineUp.position = Vector3.Lerp(cineUp.position, initUp, 0.05f);

            yield return null;
            if (timer > 0.8f) yield break;
        }
    }

    //oversee function
    private void OnChangeDragonHP(float curHp, float maxHp)
    {
        dragonHpGroupObj.SetActive(true);
        dragonHpSlider.value = curHp / maxHp;
        StartCoroutine(DragonFollowSlider());
    }

    IEnumerator DragonFollowSlider()
    {
        while (true)
        {
            dragonHpFollowSlider.value = Mathf.Lerp(dragonHpFollowSlider.value, dragonHpSlider.value, Time.deltaTime / 2f);

            if (dragonHpFollowSlider.value == dragonHpSlider.value)
            {
                yield break;
            }

            yield return null;
        }
    }

    private void OnChangedHp(float curHp, float maxHp)
    {
        hpSlider.value = curHp / maxHp;
        StartCoroutine(FollowSlider());
    }

    IEnumerator FollowSlider()
    {
        while (true)
        {
            hpFollowSlider.value = Mathf.Lerp(hpFollowSlider.value, hpSlider.value, Time.deltaTime / 5f);

            if (hpFollowSlider.value == hpSlider.value) yield break;

            yield return null;
        }
    }












    private void GameEndControll()
    {

        if (GameManager.INSTANCE.ISDEAD)
        {
            // 마우스 커서 활성화
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //calculation
            GameManager.INSTANCE.CoinRavish();

            // 게임 오버 창 띄우기
            offenseEndUI.SetActive(true);


            // 드래곤 잡은 수 체크 -> 드래곤이 죽을때마다 count 로 체크
            DragonKilledCount.text = "" + GameManager.INSTANCE.KILLCOUNT;//"KilledDragons_number" + 
            // 식물 잡은 수 체크 -> 식물이 죽을때마다 체크
            PlantKilledCount.text = "" + GameManager.INSTANCE.DESTROYPLANTCOUNT;//"StealSeeds_number" + 
            // 집 -> 없어질때마다 체크
            BuildingDestroyCount.text = "" + GameManager.INSTANCE.DESTROYBUILDINGCOUNT;//"DestroyBuilding_number" +

            TotalScore.text = "" + GameManager.INSTANCE.TOTALCOIN;
            GetCoin.text = "" + GameManager.INSTANCE.TOTALCOIN;



            // 나중에 착착 한 줄씩 점수 뜨는 효과 넣기

        }
    }

    // 버튼 누를때는 마우스 커서 활성화 필요
    public void OnBackButton()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DefenseScene");
        asyncOperation.allowSceneActivation = true;
    }
}


