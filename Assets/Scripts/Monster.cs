using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AnimatableCharacter
{
    [SerializeField]
    private int _hp = 5;
    [SerializeField]
    private int _blockedLevel = 0;
    [SerializeField]
    private int _gainExp = 10;
    [SerializeField]
    private int _gainGold = 5;
    [SerializeField]
    private int _attackPoint = 1;

    public int BlockedLevel => _blockedLevel;

    public int Hp { get; set; }
    public int AttackPoint => _attackPoint;

    public int GainExp => _gainExp;
    public int GainGold => _gainGold;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize()
    {
        Hp = _hp;
    }
}
