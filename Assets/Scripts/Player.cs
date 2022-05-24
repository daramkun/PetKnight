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
    private bool _isWalking;
    private bool _isDead;

    private int _level;
    private int _hp;
    private int _maxHp;
    private int _mp;
    private int _maxMp;
    private long _gold;
    private int _exp;

    public PlayerPointEvent PlayerHpChanged = new PlayerPointEvent();
    public PlayerPointEvent PlayerMpChanged = new PlayerPointEvent();
    public GoldChangedEvent GoldChanged = new GoldChangedEvent();

    public int Level => _level;
    public int Hp => _hp;
    public int Mp => _mp;
    public int MaxHp => _maxHp;
    public int MaxMp => _maxMp;
    public long Gold => _gold;
    public int Exp => _exp;

    public int AttackPoint => Mathf.Clamp(Mathf.RoundToInt(Mathf.Log10(_level) * 10), 1, 10000);

    protected override void Awake()
    {
        base.Awake();

        _level = 1;
        _exp = 0;

        _hp = _maxHp = 20;
        _mp = _maxMp = 10;
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
        _hp = (int)Mathf.Clamp(_hp + unit, 0, _maxHp);
        PlayerHpChanged.Invoke(_hp / (float)_maxHp);
    }

    public void SetGold(int amount)
    {
        _gold = Math.Max(0, Math.Min(long.MaxValue, _gold + amount));
        GoldChanged.Invoke(_gold);
    }

    public void SetMp(float unit)
    {
        _mp = (int)Mathf.Clamp(_mp + unit, 0, _maxMp);
        PlayerMpChanged.Invoke(_mp / (float)_maxMp);
    }

    public bool SetExp(int amount)
    {
        var prevLevel = _level;
        _exp += amount;
        int nextLvExp;
        while ((nextLvExp = CalculateNextLevelupExpPoint(_level + 1)) <= _exp)
        {
            ++_level;
            _exp -= nextLvExp;
        }
        
        Debug.LogFormat("Level Up: {0}, Exp: {1}, Amount: {2}", _level, _exp, amount);

        return _level != prevLevel;
    }

    private static int CalculateNextLevelupExpPoint(int level)
    {
        return Mathf.Clamp(Mathf.RoundToInt(Mathf.Log10(level) * (level * 100)), 100, int.MaxValue);
    }
}
