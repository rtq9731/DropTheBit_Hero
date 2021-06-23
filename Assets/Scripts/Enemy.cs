using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    [SerializeField] Animator animator;

    private int cost = 0;
    private float hp = 0;
    float timer = 0f;

    private bool isDead = false;

    private int attackHash = Animator.StringToHash("Attack");
    private int hitHash = Animator.StringToHash("Hit");
    private int dieHash = Animator.StringToHash("Die");

    private void Update()
    {
        timer += Time.deltaTime;
        if( timer % 1 == 0 && !isDead)
        {
            animator.SetTrigger(attackHash);
        }
    }

    private void OnEnable()
    {
        MoveToFightPos();
    }

    public void InitEnemy(int cost, float hp)
    {
        this.cost = cost;
        this.hp = hp;
        timer = 0f;
        isDead = false;
    }

    private void DieAction()
    {
        animator.SetTrigger(dieHash);
        GameManager.Instance.AddMoney(cost);
        GameManager.Instance.KillCount++;
        MainSceneManager.Instance.Player.RemoveEnmey();
        Invoke("Die", 0.93f);
    }

    private void Die()
    {
        MainSceneManager.Instance.CallNextEnemy();
        transform.DOMoveX(-7.75f, 2f).OnComplete(()=> {
            gameObject.SetActive(false);
            });
    }

    public void Hit(float damage)
    {
        if (isDead)
        {
            return;
        }

        hp -= damage;
        if (hp <= 0)
        {
            isDead = true;
            DieAction();
            return;
        }
        animator.SetTrigger(hitHash);
    }

    private void MoveToFightPos()
    {
        transform.DOMoveX(1f, 1f).SetEase(Ease.OutCubic).OnComplete(() => {
            animator.SetTrigger(attackHash);
            MainSceneManager.Instance.Player.SetEnmey(this);
        });
    }
}
