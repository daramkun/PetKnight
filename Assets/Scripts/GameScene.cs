using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : SingletonBehaviour<GameScene>
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private StageMarquee[] _stages;

    private GameState _gameState;
    private Stage _currentStage;

    [SerializeField]
    private Monster[] _monsters;

    private Monster _currentMonster;

    [SerializeField]
    private ObjectPool _damageTextPool;

    [SerializeField]
    private GameObject _gameOverText;

    void Awake()
    {
        _currentStage = Stage.None;
    }

    void Start()
    {
        ChangeStage(Stage.Castle);
        ChangeState(GameState.Explore);
    }

    void Update()
    {

    }

    public void DoDrinkPotion(int point)
    {
        if (_player.Gold - point < 0)
            return;
        if (Mathf.Abs(_player.Hp - _player.MaxHp) < Mathf.Epsilon)
            return;
        
        _player.SetGold(-point);
        _player.SetHp(point);
    }

    public void ChangeState(GameState gameState)
    {
        this._gameState = gameState;
        switch (gameState)
        {
            case GameState.Explore:
                foreach (var stage in _stages)
                    stage._marqueeOn = true;
                StartCoroutine(WalkingState());
                break;

            case GameState.MonsterSummon:
                _player.ChangeAnimation(AnimationType.Idle);
                foreach (var stage in _stages)
                    stage._marqueeOn = false;
                StartCoroutine(MonsterSummonState());
                break;

            case GameState.PlayerAttackPhase:
                StartCoroutine(PlayerAttackPhaseState());
                break;

            case GameState.MonsterAttackPhase:
                StartCoroutine(MonsterAttackPhaseState());
                break;

            case GameState.Ceremony:
                StartCoroutine(CeremonyState());
                break;

            case GameState.GameOver:
                StartCoroutine(GameOverState());
                break;
        }
    }

    public void ChangeStage(Stage stage)
    {
        _currentStage = stage;
        for (var i = 0; i < _stages.Length; ++i)
            _stages[i].gameObject.SetActive((int)stage == i);
    }

    private IEnumerator WalkingState()
    {
        _player.ChangeAnimation(AnimationType.Walking);

        var starting = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - starting <= 5)
            yield return null;

        ChangeState(GameState.MonsterSummon);
    }

    private IEnumerator MonsterSummonState()
    {
        var choosedMonsters = _monsters.Where(monster => _player.Level >= monster.BlockedLevel);
        var choosedMonster = Probability.GetEqualProbability(choosedMonsters);
        _currentMonster = choosedMonster;

        choosedMonster.gameObject.SetActive(true);
        choosedMonster.Initialize();
        choosedMonster.ChangeAnimation(AnimationType.Walking);

        var startTime = Time.realtimeSinceStartup;
        var startingPosition = new Vector3(0.94f * 2, -1.26f, 0);
        var targetPosition = new Vector3(0.94f, -1.26f, 0);

        var choosedMonsterTransform = choosedMonster.transform;
        choosedMonsterTransform.localPosition = startingPosition;

        while ((Time.realtimeSinceStartup - startTime) / 3 < 1)
        {
            var temp = (float)(Time.realtimeSinceStartup - startTime) / 3;
            var animationAmount = (startingPosition - targetPosition) * temp;

            choosedMonsterTransform.localPosition = startingPosition - animationAmount;

            yield return null;
        }

        choosedMonsterTransform.localPosition = targetPosition;
        choosedMonster.ChangeAnimation(AnimationType.Idle);

        ChangeState(GameState.PlayerAttackPhase);
    }

    private IEnumerator PlayerAttackPhaseState()
    {
        _player.ChangeAnimation(AnimationType.Attack);

        while (_player.Animation == AnimationType.Attack)
            yield return null;

        var missed = false;
        if (_currentMonster.HasMiss)
        {
            missed = Probability.IsMissed();
            if (missed)
            {
                var damageText = _damageTextPool.Pop();
                damageText.GetComponent<DamageText>().StartAnimation(_currentMonster.gameObject, "MISSED", Color.yellow);

                _currentMonster.ChangeAnimation(AnimationType.Miss);
                while (_currentMonster.Animation == AnimationType.Miss)
                    yield return null;
            }
        }

        if (!missed)
        {
            var damageText = _damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(_currentMonster.gameObject, _player.AttackPoint.ToString(), Color.red);
            _currentMonster.Hp -= _player.AttackPoint;

            _currentMonster.ChangeAnimation(AnimationType.Hit);
            while (_currentMonster.Animation == AnimationType.Hit)
                yield return null;
        }

        yield return CachedWaitFor.GetWaitForSeconds(1);

        ChangeState(_currentMonster.Hp <= 0
                        ? GameState.Ceremony
                        : GameState.MonsterAttackPhase);
    }

    private IEnumerator MonsterAttackPhaseState()
    {
        _currentMonster.ChangeAnimation(AnimationType.Attack);

        while (_currentMonster.Animation == AnimationType.Attack)
            yield return null;

        var missed = Probability.IsMissed();
        if (missed)
        {
            var damageText = _damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(_player.gameObject, "MISSED", Color.yellow);

            _player.ChangeAnimation(AnimationType.Miss);
            while (_player.Animation == AnimationType.Miss)
                yield return null;
        }
        else
        {
            var damageText = _damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(_player.gameObject, _currentMonster.AttackPoint.ToString(), Color.red);
            _player.SetHp(-_currentMonster.AttackPoint);

            _player.ChangeAnimation(AnimationType.Hit);
            while (_player.Animation == AnimationType.Hit)
                yield return null;
        }

        yield return CachedWaitFor.GetWaitForSeconds(1);

        ChangeState(_player.Hp <= 0
                        ? GameState.GameOver
                        : GameState.PlayerAttackPhase);
    }

    private IEnumerator CeremonyState()
    {
        _currentMonster.ChangeAnimation(AnimationType.Dead);
        yield return CachedWaitFor.GetWaitForSeconds(1);

        _player.SetGold(_currentMonster.GainGold);

        if (_player.SetExp(_currentMonster.GainExp))
        {
            var damageText = _damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(_player.gameObject, "Lv UP!", Color.green);

            yield return CachedWaitFor.GetWaitForSeconds(1);
        }

        _currentMonster.gameObject.SetActive(false);
        _currentMonster = null;

        ChangeState(GameState.Explore);
        yield break;
    }

    private IEnumerator GameOverState()
    {
        _player.ChangeAnimation(AnimationType.Dead);
        _gameOverText.SetActive(true);
        
        yield return CachedWaitFor.GetWaitForSeconds(1);

        SceneManager.LoadScene("MenuScene");
    }
}