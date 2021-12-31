using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AnimatableCharacter
{
    [SerializeField]
    private int hp = 5;
    [SerializeField]
    private int blockedLevel = 0;
    [SerializeField]
    private int gainExp = 10;
    [SerializeField]
    private int gainGold = 5;
    [SerializeField]
    private int attackPoint = 1;

    public int BlockedLevel => blockedLevel;

    public int Hp { get; set; }
    public int AttackPoint => attackPoint;

    public int GainExp => gainExp;
    public int GainGold => gainGold;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize()
    {
        Hp = hp;
    }
}
