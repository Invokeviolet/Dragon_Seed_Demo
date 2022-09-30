using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Dragon Data", menuName = "Scriptavle Object/Dragon Data",order =int.MaxValue)]
public class DragonData : ScriptableObject
{

    //타입
    [SerializeField]
    private string dragonType;
    public string DragonType { get { return dragonType; } }

    //공격력
    [SerializeField]
    private float attackPower;
    public float AttackPower { get { return attackPower; } }

    //체력
    [SerializeField]
    private float hp;
    public float HP { get { return hp; } }

    //속도
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    //크기
    [SerializeField]
    private float scale;
    public float Scale { get { return scale; } }

    //성장시간
    [SerializeField]
    private int growTime;
    public int GrowTime { get { return growTime; } }

    //등급
    [SerializeField]
    private int rare;
    public int Rare { get { return rare; } }


    // 업그레이드 
    [SerializeField]
    private float upgradePower;
    public float UpgradePower { get { return upgradePower; } }

    [SerializeField]
    private float upgradeHP;
    public float UpgradeHP { get { return upgradeHP; } }

    [SerializeField]
    private float upgradeScale;
    public float UpgradeScale { get { return upgradeScale; } }


}
