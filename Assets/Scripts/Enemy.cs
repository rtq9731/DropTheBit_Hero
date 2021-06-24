using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    [SerializeField] Animator animator;

    private float hitOffset = 0.3f;
    private float dieOffset = 0.7f;

    private int cost = 0;
    public float hp = 0;
    float timer = 0f;

    private bool isDead = false;

    private int attackHash = Animator.StringToHash("Attack");
    private int hitHash = Animator.StringToHash("Hit");
    private int dieHash = Animator.StringToHash("Die");

    private void OnEnable()
    {
        MoveToFightPos();
    }

    public void InitEnemy(RuntimeAnimatorController controller, int cost, float hp)
    {
        this.GetComponent<Animator>().runtimeAnimatorController = controller;
        this.cost = cost;
        this.hp = hp;
        timer = 0f;
        isDead = false;
    }

    private IEnumerator DieAction()
    {

        animator.SetBool(attackHash, false);
        yield return new WaitForSeconds(hitOffset);
        animator.SetBool(dieHash, true);
        MainSceneManager.Instance.UpdateHPSlider();
        yield return new WaitForSeconds(dieOffset);
        MainSceneManager.Instance.StopAttackEffect();

        GameManager.Instance.AddMoney(cost);
        GameManager.Instance.KillCount++;
        MainSceneManager.Instance.Player.RemoveEnmey();

        MainSceneManager.Instance.CallNextEnemy();
        transform.DOMoveX(-7.75f, 2f).OnComplete(() => {
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
            StartCoroutine(DieAction());
            return;
        }
        StartCoroutine(PlayHitMotion(damage));
    }

    IEnumerator PlayHitMotion(float damage)
    {
        yield return new WaitForSeconds(hitOffset);
        MainSceneManager.Instance.UpdateHPSlider();
        MainSceneManager.Instance.PlayAttackEffect();
        animator.SetTrigger(hitHash);
    }

    private void MoveToFightPos()
    {
        transform.DOMoveX(1f, 1f).SetEase(Ease.OutCubic).OnComplete(() => {
            animator.SetBool("Attack", true);
            MainSceneManager.Instance.Player.SetEnmey(this);
        });
    }
}
