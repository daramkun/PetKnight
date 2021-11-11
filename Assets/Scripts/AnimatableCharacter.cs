using System;
using UnityEngine;

public enum AnimationType
{
    Idle,

    Walking,
    Dead,

    Attack,
    Attack2,
    Hit,
    Miss,
}

[RequireComponent(typeof(Animator))]
public class AnimatableCharacter : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Miss = Animator.StringToHash("Miss");

    private Animator animator;
    public bool HasMiss { get; private set; }
    public bool HasAttack2 { get; private set; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        HasMiss = animator.HasState(0, Miss);
        HasAttack2 = animator.HasState(0, Attack2);
    }

    public AnimationType Animation
    {
        get
        {
            var nameHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

            var returnValue = (AnimationType)(-1);
            if (nameHash == Idle)
                returnValue = AnimationType.Idle;
            else if (nameHash == Walking)
                returnValue = AnimationType.Walking;
            else if (nameHash == Dead)
                returnValue = AnimationType.Dead;
            else if (nameHash == Attack1)
                returnValue = AnimationType.Attack;
            else if (nameHash == Attack2)
                returnValue = AnimationType.Attack2;
            else if (nameHash == Hit)
                returnValue = AnimationType.Hit;
            else if (nameHash == Miss)
                returnValue = AnimationType.Miss;

            Debug.LogFormat("Get Animation: {0}", returnValue);

            return returnValue;
        }
    }

    public void ChangeAnimation(AnimationType type)
    {
        Debug.LogFormat("Change Animation: {0}", type);
        switch (type)
        {
            case AnimationType.Idle:
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDead, false);
                animator.Play(Idle);
                break;

            case AnimationType.Walking:
                animator.SetBool(IsWalking, true);
                animator.SetBool(IsDead, false);
                break;

            case AnimationType.Dead:
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDead, true);
                break;

            case AnimationType.Attack:
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDead, false);
                animator.Play(Attack1);
                break;

            case AnimationType.Attack2:
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDead, false);
                animator.Play(Attack2);
                break;

            case AnimationType.Hit:
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDead, false);
                animator.Play(Hit);
                break;

            case AnimationType.Miss:
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsDead, false);
                animator.Play(Miss);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}