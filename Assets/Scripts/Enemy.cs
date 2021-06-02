using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int money;

    EnemyData enemyData = new EnemyData();

    [SerializeField] Animator animator;
    [SerializeField] Animation enemy_Hit;

    EnemyState state = EnemyState.Walk;
    EnemyState State { get { return state; } set { state = value; isStateChange = true; } }

    bool isStateChange = true;
    bool isDie = false;

    float animationTimer;
    float animationCool;

    enum EnemyState
    {
        Walk,
        Atk,
        Hit,
        Die
    };

    private void Start()
    {
        State = EnemyState.Walk;
        enemyData.HP = hp;
    }

    private void Update()
    {
        if(state == EnemyState.Die && !isDie)
        {
            isDie = true;
            animator.Play("Enemy_Die");
            MainSceneManager.Instance.GetPlayer().RemoveEnmey();
            Invoke("Die", 0.93f);
        }

        if(isStateChange)
        {
            switch (State)
            {
                case EnemyState.Walk:
                    MoveToFightPos();
                    break;
                case EnemyState.Atk:
                    break;
                case EnemyState.Die:
                    break;
                case EnemyState.Hit:
                    break;
                default:
                    break;
            }
        }

    }

    private void Die()
    {
        GameManager.Instance.AddMoney(money);
        MainSceneManager.Instance.CallNextEnmey();
        transform.DOMoveX(-7.75f, 2f).OnComplete(()=> {
            Destroy(this.gameObject);
            });
    }

    public void Hit(float damage)
    {
        if (enemyData.HP <= 0)
        {
            State = EnemyState.Die;
            return;
        }
        animator.Play("Enemy_Hit"); 
        enemyData.HP -= damage;
        State = EnemyState.Hit;
    }

    private void MoveToFightPos()
    {
        isStateChange = false;
        transform.DOMoveX(1f, 2f).SetEase(Ease.Linear).OnComplete(() => { 
            State = EnemyState.Atk;
            MainSceneManager.Instance.GetPlayer().SetEnmey(this);
        });
    }
}
