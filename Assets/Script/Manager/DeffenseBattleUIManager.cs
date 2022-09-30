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

        // �����̴� �ʱⰪ ����
        stamina_Slider.value = stamina_Slider.maxValue;

        defenseUserAttackPower = defenseUserMaxAttackPower;

        sliderRechargeTime += Time.deltaTime / 100;

    }





    private void Start()
    {
        GameManager.INSTANCE.TimerStart();

        // ���� ���� ���� ������ ��� �ð� ����
        if (GameManager.INSTANCE.ISDEAD == true)
        {
            GameManager.INSTANCE.TimeOut();
        }

        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit))
        {
            Debug.Log("����ĳ��Ʈ �߻�!");
        }

    }





    private void Update()
    {

        // �÷��� Ÿ�� �ؽ�Ʈ�� �޾ƿ���
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
        // ���콺 ��ư �Է��� ���� ���
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

        // Ǯ������ ��ų ������Ʈ ����
        GameObject skill = ObjectPoolingManager.inst.Instantiate(attackSkillPrefab, pos + new Vector3(0, 0.5f, 0), Quaternion.identity, tr);

        yield return new WaitForSeconds(1f);
        ObjectPoolingManager.inst.Destroy(skill);

        // ���¹̳ʰ� 0���� �϶��� ���� �Ұ�
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
        // ���¹̳� �����̴�
        Debug.Log($"{defenseUserAttackPower} ������ ��ŭ ����");

        // ���¹̳� ���
        stamina_Slider.value -= 0.2f;
    }

    #endregion

}


