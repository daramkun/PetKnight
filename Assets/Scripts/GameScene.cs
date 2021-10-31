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
                StartCoroutine(WalkingState());
                break;

            case GameState.MonsterSummon:
                player.ChangeAnimation(AnimationType.Idle);
                foreach (var stage in stages)
                    stage.marqueeOn = false;
                StartCoroutine(MonsterSummonState());
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

        choosedMonster.gameObject.SetActive(true);
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
}