using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    PlayerData player = new PlayerData();
    Animator playerAnimator;

    [SerializeField] float atk;

    private Enemy enemy;

    private float atkTimer;

    private bool isFirstFight = true;

    enum PlayerState
    {
        Walk,
        Atk
    };

    PlayerState state;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        state = PlayerState.Walk;
        player.ATK = this.atk;
    }

    private void Update()
    {
        switch (state)
        {
            case PlayerState.Walk:

                if(isFirstFight)
                {
                    MoveToFightPos();
                    playerAnimator.SetBool("isWalk", true);
                }
                break;

            case PlayerState.Atk:
                Fight();
                break;

            default:
                break;
        }
    }

    private void MoveToFightPos()
    {
        if (playerAnimator.GetBool("isWalk"))
        {
            return;
        }

        transform.DOMoveX(-0.75f, 2f).OnComplete(() => { state = PlayerState.Atk; playerAnimator.SetBool("isWalk", false); playerAnimator.SetBool("isAttack", true); MainSceneManager.Instance.ScrollingBackground(); });
    }

    private void Fight()
    {
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).length <= atkTimer)
        {
            this.enemy.Hit(this.player.ATK);
        }
        atkTimer += Time.deltaTime;
    }

    public void SetEnmey(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void RemoveEnmey()
    {
        this.enemy = null;
        state = PlayerState.Walk;
    }
}
