using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    Animator animator;

    private float hitOffset = 0.3f;
    private float dieOffset = 0.7f;

    [SerializeField] public Transform hpBarPos = null;
    [SerializeField] public Transform namePanelPos = null;
    [SerializeField] public Transform effectPos = null;

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

    public void InitEnemy(int cost, float hp)
    {
        MainSceneManager.Instance.UpdateHPSlider();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load(GameManager.Instance.EnemyDatas[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]].Animatorcontrollerpath) as RuntimeAnimatorController;
        if(GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex].Contains("슬라임")) // 왼쪽 방향으로 걸어가는 적의 경우엔
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex].Contains("해골"))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else // 오른쪽 방향으로 걷는 적인 경우에
        {
            GetComponent<SpriteRenderer>().flipX = true;
            hpBarPos.localPosition = new Vector2(0, -0.25f);
            namePanelPos.localPosition = new Vector2(0, 1f);
        }
        animator = GetComponent<Animator>();
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
