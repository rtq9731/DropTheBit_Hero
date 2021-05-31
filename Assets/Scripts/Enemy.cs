using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hp;
    EnemyData enemyData = new EnemyData();
    [SerializeField] Animator animator;

    EnemyState state = EnemyState.Walk;
    EnemyState State { get { return state; } set { state = value; isStateChange = true; } }

    bool isStateChange = true;
    float destroyTimer = 0f;

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
        if(isStateChange)
        {
            switch (State)
            {
                case EnemyState.Walk:
                    ClearAnimator();
                    MoveToFightPos();
                    animator.SetBool("isWalk", true);
                    break;
                case EnemyState.Atk:
                    ClearAnimator();
                    animator.SetBool("isAttack", true);
                    break;
                case EnemyState.Die:
                    ClearAnimator();
                    animator.SetBool("isDie", true);
                    Invoke("Die", 0.93f);
                    break;
                case EnemyState.Hit:
                    ClearAnimator();
                    animator.SetBool("isHit", true);
                    break;
                default:
                    break;
            }
        }

    }

    private void Die()
    {
        GameManager.Instance.AddMoney(30);
        MainSceneManager.Instance.GetPlayer().RemoveEnmey();
        Destroy(this.gameObject);
    }

    public void Hit(float damage)
    {
        if (enemyData.HP <= 0)
        {
            State = EnemyState.Die;
            return;
        }
        enemyData.HP -= damage;
        State = EnemyState.Hit;
    }

    private void MoveToFightPos()
    {
        MainSceneManager.Instance.GetPlayer().SetEnmey(this);
        transform.DOMoveX(0.75f, 2f).OnComplete(() => { State = EnemyState.Atk; animator.SetBool("isWalk", false); animator.SetBool("isAttack", true);});
    }

    public void ClearAnimator()
    {
        animator.SetBool("isHit", false);
        animator.SetBool("isDie", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalk", false);
        isStateChange = false;
    }
}
