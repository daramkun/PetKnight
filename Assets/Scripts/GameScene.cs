using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScene : SingletonBehaviour<GameScene>
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private StageMarquee[] stages;

    private GameState gameState;
    private Stage currentStage;

    [SerializeField]
    private Monster[] monsters;

    private Monster currentMonster;

    [SerializeField]
    private ObjectPool damageTextPool;

    void Awake()
    {
        currentStage = Stage.None;
    }

    void Start()
    {
        ChangeStage(Stage.Castle);
        ChangeState(GameState.Explore);
    }

    void Update()
    {

    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
        switch (gameState)
        {
            case GameState.Explore:
                foreach (var stage in stages)
                    stage.marqueeOn = true;
                StartCoroutine(WalkingState());
                break;

            case GameState.MonsterSummon:
                player.ChangeAnimation(AnimationType.Idle);
                foreach (var stage in stages)
                    stage.marqueeOn = false;
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
                break;
        }
    }

    public void ChangeStage(Stage stage)
    {
        currentStage = stage;
        for (var i = 0; i < stages.Length; ++i)
            stages[i].gameObject.SetActive((int)stage == i);
    }

    private IEnumerator WalkingState()
    {
        player.ChangeAnimation(AnimationType.Walking);

        var starting = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - starting <= 5)
            yield return null;

        ChangeState(GameState.MonsterSummon);
    }

    private IEnumerator MonsterSummonState()
    {
        var choosedMonsters = monsters.Where(monster => player.Level >= monster.BlockedLevel);
        var choosedMonster = Probability.GetEqualProbability(choosedMonsters);
        currentMonster = choosedMonster;

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
        player.ChangeAnimation(AnimationType.Attack);

        while (player.Animation == AnimationType.Attack)
            yield return null;

        var missed = false;
        if (currentMonster.HasMiss)
        {
            missed = Probability.IsMissed();
            if (missed)
            {
                var damageText = damageTextPool.Pop();
                damageText.GetComponent<DamageText>().StartAnimation(currentMonster.gameObject, "MISSED", Color.yellow);

                currentMonster.ChangeAnimation(AnimationType.Miss);
                while (currentMonster.Animation == AnimationType.Miss)
                    yield return null;
            }
        }

        if (!missed)
        {
            var damageText = damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(currentMonster.gameObject, player.AttackPoint.ToString(), Color.red);
            currentMonster.Hp -= player.AttackPoint;

            currentMonster.ChangeAnimation(AnimationType.Hit);
            while (currentMonster.Animation == AnimationType.Hit)
                yield return null;
        }

        yield return CachedWaitFor.GetWaitForSeconds(1);

        ChangeState(currentMonster.Hp <= 0
                        ? GameState.Ceremony
                        : GameState.MonsterAttackPhase);
    }

    private IEnumerator MonsterAttackPhaseState()
    {
        currentMonster.ChangeAnimation(AnimationType.Attack);

        while (currentMonster.Animation == AnimationType.Attack)
            yield return null;

        var missed = Probability.IsMissed();
        if (missed)
        {
            var damageText = damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(player.gameObject, "MISSED", Color.yellow);

            player.ChangeAnimation(AnimationType.Miss);
            while (player.Animation == AnimationType.Miss)
                yield return null;
        }
        else
        {
            var damageText = damageTextPool.Pop();
            damageText.GetComponent<DamageText>().StartAnimation(player.gameObject, currentMonster.AttackPoint.ToString(), Color.red);
            player.SetHp(-currentMonster.AttackPoint);

            player.ChangeAnimation(AnimationType.Hit);
            while (player.Animation == AnimationType.Hit)
                yield return null;
        }

        yield return CachedWaitFor.GetWaitForSeconds(1);

        ChangeState(player.Hp <= 0
                        ? GameState.GameOver
                        : GameState.PlayerAttackPhase);
    }

    private IEnumerator CeremonyState()
    {
        currentMonster.ChangeAnimation(AnimationType.Dead);
        yield return CachedWaitFor.GetWaitForSeconds(1);

        player.SetExp(currentMonster.GainExp);
        player.SetGold(currentMonster.GainGold);

        currentMonster.gameObject.SetActive(false);
        currentMonster = null;

        ChangeState(GameState.Explore);
        yield break;
    }

    private IEnumerator GameOverState()
    {
        player.ChangeAnimation(AnimationType.Dead);
        yield return CachedWaitFor.GetWaitForSeconds(1);
    }
}