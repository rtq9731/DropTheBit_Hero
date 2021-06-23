using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossSceneManager : MonoBehaviour
{
    [Header("싸움 관련")]
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Slider progressBar = null;

    [Header("전투 결과 관련")]
    [SerializeField] GameObject bgPanel;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Button btnChangeScene;


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
    /// 보스와의 전투가 끝난 후, 결과에 따라 알맞은 행동을 해줄 메소드에요.
    /// </summary>

    public void FinishFight()
    {
        bgPanel.SetActive(true);
        resultPanel.SetActive(true);
        btnChangeScene.onClick.AddListener(() => GameManager.Instance.ChangeSceneToMainScene());
    }
}
