using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerPointEvent : UnityEvent<float>
{

}

[Serializable]
public class GoldChangedEvent : UnityEvent<long>
{

}

public class Player : AnimatableCharacter
{
    private bool isWalking;
    private bool isDead;

    private int level;
    private int hp;
    private int maxHp;
    private int mp;
    private int maxMp;
    private long gold;
    private int exp;

    public PlayerPointEvent PlayerHpChanged = new PlayerPointEvent();
    public PlayerPointEvent PlayerMpChanged = new PlayerPointEvent();
    public GoldChangedEvent GoldChanged = new GoldChangedEvent();

    public int Level => level;
    public int Hp => hp;
    public int Mp => mp;
    public int MaxHp => maxHp;
    public int MaxMp => maxMp;
    public long Gold => gold;
    public int Exp => exp;

    public int AttackPoint => Mathf.Clamp(Mathf.RoundToInt(Mathf.Log10(level) * 10), 1, 10000);

    protected override void Awake()
    {
        base.Awake();

        level = 1;
        exp = 0;

        hp = maxHp = 20;
        mp = maxMp = 10;
    }

    void Update()
    {
        var keyboard = InputSystem.GetDevice<Keyboard>();
        if (keyboard.rightCtrlKey.IsPressed())
            PlayerHpChanged.Invoke(0.5f);
        if (keyboard.leftCtrlKey.IsPressed())
            GoldChanged.Invoke(10000);
    }

    public void SetHp(float unit)
    {
        hp = (int)Mathf.Clamp(hp + unit, 0, maxHp);
        PlayerHpChanged.Invoke(hp / (float)maxHp);
    }

    public void SetGold(int amount)
    {
        gold = Math.Max(0, Math.Min(long.MaxValue, gold + amount));
        GoldChanged.Invoke(gold);
    }

    public void SetMp(float unit)
    {
        mp = (int)Mathf.Clamp(mp + unit, 0, maxMp);
        PlayerMpChanged.Invoke(mp / (float)maxMp);
    }

    public bool SetExp(int amount)
    {
        var prevLevel = level;
        exp += amount;
        int nextLvExp;
        while ((nextLvExp = CalculateNextLevelupExpPoint(level + 1)) <= exp)
        {
            ++level;
            exp -= nextLvExp;
        }
        
        Debug.LogFormat("Level Up: {0}, Exp: {1}, Amount: {2}", level, exp, amount);

        return level != prevLevel;
    }

    private static int CalculateNextLevelupExpPoint(int level)
    {
        return Mathf.Clamp(Mathf.RoundToInt(Mathf.Log10(level) * (level * 100)), 100, int.MaxValue);
    }
}
