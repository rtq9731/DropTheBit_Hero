using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffectObj : MonoBehaviour
{
    [SerializeField] Animator animator;

    public Transform weaponAttackPos = null;

    void Update()
    {
        this.transform.position = weaponAttackPos.position;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Effect_noSprite"))
        {
            gameObject.SetActive(false);
        }
    }
}
