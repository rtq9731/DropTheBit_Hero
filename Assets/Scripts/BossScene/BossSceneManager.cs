using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSceneManager : MonoBehaviour
{
    [Header("½Î¿ò °ü·Ã")]
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public Animator playerAnimator;

    public static BossSceneManager Instance = null;

    public int bossAttack1Hash = 0;
    public int bossAttack2Hash = 0;

    private void Awake()
    {
        bossAttack1Hash = Animator.StringToHash("Attack1");
        bossAttack2Hash = Animator.StringToHash("Attack2");
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    [SerializeField] public Slider hpSlider = null;
}
