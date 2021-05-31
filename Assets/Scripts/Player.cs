using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    PlayerData player = new PlayerData();
    Animator playerAnimator;

    private bool isFirstFight = true;

    enum playerState
    {
        Walk,
        Atk
    };

    playerState state;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        state = playerState.Walk;
    }

    private void Update()
    {

        switch (state)
        {
            case playerState.Walk:

                if(isFirstFight)
                {
                    MoveToFightPos();
                }

                playerAnimator.SetBool("isWalk", true);
                break;

            case playerState.Atk:

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

        transform.DOMoveX(-0.75f, 2f).OnComplete(() => { state = playerState.Atk; playerAnimator.SetBool("isWalk", false); });
    }


}
