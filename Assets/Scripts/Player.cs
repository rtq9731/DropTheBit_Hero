using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    PlayerData player = new PlayerData();
    Animator animator;

    [SerializeField] float atk;
    [SerializeField] float attackCool;

    private Enemy enemy;

    private float atkTimer;

    private bool isFirstFight = true;
    private bool isStateChange = true;

    enum PlayerState
    {
        Walk,
        Atk
    };

    PlayerState state;
    PlayerState State { get { return state; } set { state = value; isStateChange = true; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        state = PlayerState.Walk;
        player.ATK = this.atk;
    }

    private void Update()
    {
        if(isStateChange)
        {
            switch (state)
            {
                case PlayerState.Walk:

                    if(isFirstFight)
                    {
                        MoveToFight();
                    }

                    Clearanimator();
                    break;
                case PlayerState.Atk:

                    break;
                default:
                    break;
            }
        }

        if(state == PlayerState.Atk)
        {
            Fight();
        }
    }

    private void MoveToFight()
    {
        transform.DOMoveX(-1f, 2f).SetEase(Ease.OutCubic).OnComplete(() => { 
            animator.SetBool("isAttack", true);
            State = PlayerState.Atk;
        });
    }

    private void Fight()
    {
        if(enemy == null)
        {
            Clearanimator();
            State = PlayerState.Walk;
            return;
        }

        animator.Play("Player_Attack");
        if (attackCool <= atkTimer)
        {
            this.enemy.Hit(this.player.ATK);
            atkTimer = 0;
        }

        atkTimer += Time.deltaTime;
    }

    public void Clearanimator()
    {
        animator.SetBool("isAttack", false);
        isStateChange = false;
    }

    public void SetEnmey(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void RemoveEnmey()
    {
        this.enemy = null;
    }
}
