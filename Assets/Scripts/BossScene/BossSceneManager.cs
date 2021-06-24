using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class BossSceneManager : MonoBehaviour
{
    [Header("�ο� ����")]
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Slider progressBar = null;

    [Header("���� ��� �� Ŭ��Ŀ")]
    [SerializeField] RhythmManager rhythmManager;
    [SerializeField] GameObject originCam;
    [SerializeField] GameObject closeUpCam;
    [SerializeField] Slider bossHPBar;
    [SerializeField] Text clickerText;

    [Header("���� ��� ����")]
    [SerializeField] GameObject bgPanel;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Button btnChangeScene;

    public static BossSceneManager Instance = null;

    float clickerTime = 30f;

    public int bossAttack1Hash = 0;
    public int bossAttack2Hash = 0;

    public int playerAttackHash = 0;
    public int playerHitHash = 0;

    private bool isActiveCliker = false;

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
        ClearBossScene();
        StartClicker();
    }

    void ClearBossScene()
    {
        rhythmManager.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        originCam.SetActive(false);
    }

    void StartClicker()
    {
        isActiveCliker = true; // Cliker ON!
        bossHPBar.maxValue = progressBar.maxValue * 2;
        Debug.Log(bossHPBar.maxValue);
        bossHPBar.value = bossHPBar.maxValue;
        bossHPBar.gameObject.SetActive(true);
        playerAnimator.transform.position = new Vector2(-2, -2.5f);
        bossAnimator.transform.position = new Vector2(2, -2.5f);
        closeUpCam.SetActive(true);
    }

    private void Update()
    {
        if (isActiveCliker)
        {
            clickerText.text = $"���� Ŭ���� ������ ���̼���!\n�����ð�: {Mathf.Round(clickerTime * 1000) * 0.001f}s";

            if (clickerTime <= 0)
                FinishClicker();

            if (Input.GetMouseButtonDown(0))
            {
                bossHPBar.value--;
                if(bossHPBar.value <= 0)
                {
                    FinishClicker();
                }
            }

            clickerTime -= Time.deltaTime;
        }
    }



    private void FinishClicker()
    {
        bgPanel.SetActive(true);
        resultPanel.SetActive(true);
        btnChangeScene.onClick.AddListener(() => GameManager.Instance.ChangeSceneToMainScene());
    }
}
