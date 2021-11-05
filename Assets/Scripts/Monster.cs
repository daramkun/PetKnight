using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AnimatableCharacter
{
    [SerializeField]
    private int hp = 20;
    [SerializeField]
    private int blockedLevel = 0;
    [SerializeField]
    private int gainExp = 10;

    public int BlockedLevel => blockedLevel;

    public int Hp { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize()
    {
        Hp = hp;
    }
}
