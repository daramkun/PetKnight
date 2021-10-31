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

    protected override void Awake()
    {
        base.Awake();
    }
}
