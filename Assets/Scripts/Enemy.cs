using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    MonsterDataData data;

    [SerializeField] Animator animator;
    [SerializeField] Animation enemy_Hit;
    [SerializeField] string Name;

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
    }

    private void OnEnable()
    {
        StartCoroutine(InputData());
    }

    private void Update()
    {
        if(state == EnemyState.Die && !isDie)
        {
            isDie = true;
            animator.Play("Enemy_Die");
            GameManager.Instance.AddMoney(data.Cost);
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
        MainSceneManager.Instance.CallNextEnmey();
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
        transform.DOMoveX(1f, 2f).SetEase(Ease.Linear).OnComplete(() => { 
            State = EnemyState.Atk;
            MainSceneManager.Instance.GetPlayer().SetEnmey(this);
        });
    }

    private IEnumerator InputData()
    {
        string[] temp = name.Split('(');
        string tempName = temp[0];
        Debug.Log(tempName);
        yield return new WaitForSeconds(0.1f);
        data = MainSceneManager.Instance.GetEnemyDataFromName(tempName);
        yield return null;
    }
}
