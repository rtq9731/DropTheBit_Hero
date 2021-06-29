using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Animator animator;

    [SerializeField] float atk;
    public float ATK { get { return atk; } set { atk = value; } }
    [SerializeField] float attackCool;

    private Enemy enemy;

    private float atkTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        MoveToFight();
    }

    private void Update()
    {
        if (attackCool <= atkTimer)
            Attack();

        atkTimer += Time.deltaTime;
    }

    private void MoveToFight()
    {
        transform.DOMoveX(-1f, 2f).SetEase(Ease.OutCubic).OnComplete(() => {
            Attack();
        });
    }

    void Attack()
    {
        if (enemy == null)
            return;

        animator.SetTrigger("Attack");
        this.enemy.Hit(ATK);

        atkTimer = 0;
    }

    public void SetEnmey(Enemy enemy)
    {
        atkTimer = attackCool;
        this.enemy = enemy;
    }

    public void RemoveEnmey()
    {
        animator.SetTrigger("Walk");
        this.enemy = null;
    }
}
