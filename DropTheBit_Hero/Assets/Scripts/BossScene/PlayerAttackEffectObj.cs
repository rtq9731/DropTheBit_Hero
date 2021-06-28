using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffectObj : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Exit"))
        {
            gameObject.SetActive(false);
        }
    }
}
