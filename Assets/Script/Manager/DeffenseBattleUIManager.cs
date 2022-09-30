using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using System.Threading;
using Photon.Pun.Demo.PunBasics;
using UnityEditor.Experimental.GraphView;


public class DeffenseBattleUIManager : MonoBehaviour
{

    // Defense User
    #region Defense User
    [Header("[Defense User Stats]")]
    [SerializeField] private float defenseUserAttackPower;
    [SerializeField] private float defenseUserMaxAttackPower = 10f;
    [SerializeField] GameObject attackSkillPrefab;



    #endregion

    // Components
    #region components
    // Play Time
    [SerializeField] TextMeshProUGUI playTimeText;
    [SerializeField] float sliderRechargeTime = 0;

    // Stamina Slider
    [SerializeField] Slider stamina_Slider;
    #endregion

    // RaycastHit
    RaycastHit hit;







    private void Awake()
    {
        playTimeText = GameObject.Find("PlayTime_number").GetComponent<TextMeshProUGUI>();
        stamina_Slider = GameObject.Find("Stamina_Slider").GetComponent<Slider>();

        // 슬라이더 초기값 설정
        stamina_Slider.value = stamina_Slider.maxValue;

        defenseUserAttackPower = defenseUserMaxAttackPower;

        sliderRechargeTime += Time.deltaTime / 100;

    }





    private void Start()
    {
        GameManager.INSTANCE.TimerStart();

        // 만약 게임 오버 상태일 경우 시간 멈춤
        if (GameManager.INSTANCE.ISDEAD == true)
        {
            GameManager.INSTANCE.TimeOut();
        }

        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit))
        {
            Debug.Log("레이캐스트 발사!");
        }

    }





    private void Update()
    {

        // 플레이 타임 텍스트로 받아오기
        playTimeText.text = ((int)GameManager.INSTANCE.GAMETIME).ToString();

        if (stamina_Slider.value > 0.3f && Input.GetMouseButtonDown(0))
        {
            SkillCheck();
            StaminaSliderCheck();
        }

        stamina_Slider.value += sliderRechargeTime;
    }



    #region SkillCheck

    void SkillCheck()
    {
        // 마우스 버튼 입력이 있을 경우
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            StartCoroutine(CheckSkill(hit.transform.position, Quaternion.identity, this.transform));            
        }
    }

    // Instance ObjectPrefab
    IEnumerator CheckSkill(Vector3 pos, Quaternion rotation, Transform tr)
    {

        // 풀링에서 스킬 오브젝트 생성
        GameObject skill = ObjectPoolingManager.inst.Instantiate(attackSkillPrefab, pos + new Vector3(0, 0.5f, 0), Quaternion.identity, tr);

        yield return new WaitForSeconds(1f);
        ObjectPoolingManager.inst.Destroy(skill);

        // 스태미너가 0이하 일때는 공격 불가
        if (stamina_Slider.value <= 0.2f)
        {
            stamina_Slider.value = 0.2f;
            defenseUserAttackPower = 0;           
        }

    }

    #endregion



    #region StaminaSliderCheck

    void StaminaSliderCheck()
    {
        // 스태미너 슬라이더
        Debug.Log($"{defenseUserAttackPower} 데미지 만큼 공격");

        // 스태미너 사용
        stamina_Slider.value -= 0.2f;
    }

    #endregion

}


