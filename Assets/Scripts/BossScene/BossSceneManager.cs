using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossSceneManager : MonoBehaviour
{
    [Header("�ο� ����")]
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Slider progressBar = null;

    [Header("���� ��� ����")]
    [SerializeField] GameObject bgPanel;


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

        Screen.SetResolution(1440, 2560, true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;

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
