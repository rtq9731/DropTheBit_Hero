using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSceneManager : MonoBehaviour
{
    [Header("�ο� ����")]
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Slider progressBar = null;

    public static BossSceneManager Instance = null;

    public int bossAttack1Hash = 0;
    public int bossAttack2Hash = 0;

    public int playerAttackHash = 0;
    public int playerHitHash = 0;

    private void Awake()
    {
        bossAttack1Hash = Animator.StringToHash("Attack1");
        bossAttack2Hash = Animator.StringToHash("Attack2");
        playerAttackHash = Animator.StringToHash("Attack");
        playerHitHash = Animator.StringToHash("Hit");

        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// �������� ������ ���� ��, ����� ���� �˸��� �ൿ�� ���� �޼ҵ忡��.
    /// </summary>

    public void FinishFight()
    {

    }
}
