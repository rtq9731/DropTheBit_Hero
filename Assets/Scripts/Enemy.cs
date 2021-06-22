using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] MonsterData data;

    EnemyState state = EnemyState.Walk;
    EnemyState State { get { return state; } set { state = value; isStateChange = true; } }

    bool isStateChange = true;
    bool isDie = false;

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
    }

    private void OnEnable()
    {
        data = new MonsterData();
        StartCoroutine(InputData());
    }

    private void Update()
    {
        if (state == EnemyState.Die && !isDie)
        {
            isDie = true;
            animator.Play("Enemy_Die");
            GameManager.Instance.AddMoney(data.Cost);
            GameManager.Instance.KillCount++;
            MainSceneManager.Instance.Player.RemoveEnmey();
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
        GameManager.Instance.CallNextEnmey(data.Name);
        transform.DOMoveX(-7.75f, 2f).OnComplete(()=> {
            Destroy(this.gameObject);
            });
    }

    public void Hit(float damage)
    {
        if (data.HP <= 0)
        {
            State = EnemyState.Die;
            return;
        }
        animator.Play("Enemy_Hit");
        data.AddDamage(damage);
        State = EnemyState.Hit;
    }

    private void MoveToFightPos()
    {
        isStateChange = false;
        transform.DOMoveX(1f, 1f).SetEase(Ease.OutCubic).OnComplete(() => { 
            State = EnemyState.Atk;
            MainSceneManager.Instance.Player.SetEnmey(this);
        });
    }

    private IEnumerator InputData()
    {
        string[] temp = name.Split('(');
        string tempName = temp[0];
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.GetEnemyDataFromName(tempName, out float hp, out int cost);
        data.InitData(hp, cost, tempName);
        yield return null;
    }
}
