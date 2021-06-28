using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class BossSceneManager : MonoBehaviour
{
    [Header("싸움 관련")]
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Slider progressBar = null;

    [Header("전투 결과 전 클리커")]
    [SerializeField] RhythmManager rhythmManager;
    [SerializeField] GameObject originCam;
    [SerializeField] GameObject closeUpCam;
    [SerializeField] Slider bossHPBar;
    [SerializeField] Text clickerText;
    [SerializeField] AudioSource audio;
    [SerializeField] Transform soundPool;
    [SerializeField] Transform attackPool;
    [SerializeField] GameObject soundObj;
    [SerializeField] GameObject attackAnimObj;

    Queue<GameObject> attackSoundEffectPool;
    Queue<GameObject> attackEffectPool;

    [Header("전투 결과 관련")]
    [SerializeField] GameObject bgPanel;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Button btnChangeScene;
    [SerializeField] AudioClip[] audioClips;

    public static BossSceneManager Instance = null;

    private Vector2 playerStartPos = Vector2.zero;
    private Vector2 bossStratPos = Vector2.zero;

    float clickerTime = 30f;

    public int bossAttack1Hash = 0;
    public int bossAttack2Hash = 0;
    private int bossHitHash = 0;
    private int bossDeadHash = 0;

    public int playerAttackHash = 0;
    public int playerHitHash = 0;

    int attackEffectHash = 0;

    private bool isActiveCliker = false;

    private void Awake()
    {
        bossAttack1Hash = Animator.StringToHash("Attack1");
        bossAttack2Hash = Animator.StringToHash("Attack2");
        bossHitHash = Animator.StringToHash("Hit");
        bossDeadHash = Animator.StringToHash("Dead");
        playerAttackHash = Animator.StringToHash("Attack");
        playerHitHash = Animator.StringToHash("Hit");
        attackEffectHash = Animator.StringToHash("DOEffect");
        //Debug.LogWarning(bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle")); -> 지금 상태 비교하는 연산
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
        clickerText.gameObject.SetActive(true);
        clickerText.DOText($"빨리 클릭해 보스를 죽이세욧!\n남은시간: {Mathf.Round(clickerTime * 1000) * 0.001f}s", 1f).OnComplete(() => {
            isActiveCliker = true;
            bossHPBar.maxValue = progressBar.maxValue * 2;
            bossHPBar.value = bossHPBar.maxValue;
            bossHPBar.gameObject.SetActive(true);
            playerAnimator.transform.position = new Vector2(-4, -3);
            bossAnimator.transform.position = new Vector2(2, -4);
            playerStartPos = playerAnimator.gameObject.transform.position;
            bossStratPos = bossAnimator.gameObject.transform.position;
            closeUpCam.SetActive(true);
        }); // Cliker ON!
    }

    private void Update()
    {
        if (isActiveCliker)
        {
            clickerText.text = $"빨리 클릭해 보스를 죽이세욧!\n남은시간: {Mathf.Round(clickerTime * 1000) * 0.001f}s";

            if (clickerTime <= 0)
                FinishClicker(false);

            if (Input.GetMouseButtonDown(0))
            {
                bossHPBar.value -= GameManager.Instance.Atk / 20;
                if(bossHPBar.value <= 0)
                {
                    FinishClicker(true);
                }

                FightSequnce();
            }

            clickerTime -= Time.deltaTime;
        }
    }

    void MakePlayerAttackEffect()
    {
        GameObject current = null;
        if(attackEffectPool.Count > 0)
        {
            if(attackEffectPool.Peek().activeSelf)
            {
                current = Instantiate(attackAnimObj, attackPool);
            }
            else
            {
                current = attackEffectPool.Dequeue();
            }
        }
        else
        {
            current = Instantiate(attackAnimObj, attackPool);
        }

        current.SetActive(true);
        current.GetComponent<Animator>().SetTrigger(attackEffectHash);
        attackEffectPool.Enqueue(current);

    }

    void FightSequnce()
    {
        playerAnimator.gameObject.transform.DOComplete();
        bossAnimator.gameObject.transform.DOComplete();
        playerAnimator.gameObject.transform.DOMove(new Vector2(-1f, -3), 0.25f).OnComplete(() => playerAnimator.gameObject.transform.DOMove(playerStartPos, 0.25f));
        bossAnimator.gameObject.transform.DOMove(new Vector2(0.2f, -4), 0.25f).OnComplete(() => bossAnimator.gameObject.transform.DOMove(bossStratPos, 0.25f));
        audio.clip = audioClips[Random.Range(0, audioClips.Length)];
        audio.Play();
    }


    private void FinishClicker(bool isClear)
    {
        isActiveCliker = false;
        bgPanel.SetActive(true);
        resultPanel.SetActive(true);
        clickerText.gameObject.SetActive(false);
        btnChangeScene.onClick.AddListener(() => GameManager.Instance.ChangeSceneToMainScene(isClear));
    }
}
