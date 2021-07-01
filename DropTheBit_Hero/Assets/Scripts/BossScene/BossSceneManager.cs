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
    [SerializeField] Transform soundPool;
    [SerializeField] Transform attackPool;
    [SerializeField] Transform textPool;
    [SerializeField] GameObject soundObj;
    [SerializeField] GameObject attackAnimObj;
    [SerializeField] GameObject damageTextObj;
    [SerializeField] Transform bossTextEffectPos;
    [SerializeField] Transform playerAttackPos;
    [SerializeField] Transform cameraPos;
    [SerializeField] float cameraShakePower = 5f;
    [SerializeField] float cameraShakeTime = 0.1f;

    Queue<GameObject> attackSoundEffectPool = new Queue<GameObject>();
    Queue<GameObject> soundEffectPool = new Queue<GameObject>();
    Queue<GameObject> attackEffectPool = new Queue<GameObject>();
    Queue<GameObject> damageTextPool = new Queue<GameObject>();

    [Header("전투 결과 관련")]
    [SerializeField] GameObject bgPanel;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Button btnChangeScene;
    [SerializeField] AudioClip[] audioClips;

    public static BossSceneManager Instance = null;

    private Vector2 playerStartPos = Vector2.zero;
    private Vector2 bossStratPos = Vector2.zero;

    private Vector2 currentPlayerPos = Vector2.zero;
    private Vector2 currentBossPos = Vector2.zero;

    private Vector2 cameraStartPos = Vector2.zero;

    float moveTime = 0f; // Cos && Sin의 값을 넣어주기 위한 변수
    float clickerTime = 30f;

    public int bossAttack1Hash = 0;
    public int bossAttack2Hash = 0;
    private int bossHitHash = 0;
    private int bossDeadHash = 0;

    public int playerAttackHash = 0;
    public int playerHitHash = 0;

    private bool isActiveCliker = false;
    private bool isTime = false;

    private void Awake()
    {
        bossAttack1Hash = Animator.StringToHash("Attack1");
        bossAttack2Hash = Animator.StringToHash("Attack2");
        bossHitHash = Animator.StringToHash("Hit");
        bossDeadHash = Animator.StringToHash("Dead");
        playerAttackHash = Animator.StringToHash("Attack");
        playerHitHash = Animator.StringToHash("Hit");
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
        if (rhythmManager.GetRatePercent() <= 70f)
        {
            FinishClicker(false);
        }

        clickerText.gameObject.SetActive(true);
        clickerText.DOText($"빨리 클릭해 보스를 죽이세욧!\n남은시간: {clickerTime.ToString("N3")}s", 1f).OnComplete(() => {
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
            if(isTime)
            {
                playerAnimator.transform.position = new Vector2(playerStartPos.x, playerStartPos.y + Mathf.Sin(moveTime * 10));
                bossAnimator.transform.position = new Vector2(bossStratPos.x, bossStratPos.y + Mathf.Cos(moveTime * 10));

                currentPlayerPos = playerAnimator.transform.position;
                currentBossPos = bossAnimator.transform.position;

                moveTime += Time.deltaTime;
            }

            clickerText.text = $"빨리 클릭해 보스를 죽이세욧!\n남은시간: {clickerTime.ToString("N3")}s";

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

    void MakeBossDamageRate()
    {
        GameObject current;
        if(rhythmManager.textEffectPool.Count <= 0)
        {
            current = Instantiate(damageTextObj, textPool);
        }
        else
        {
            if(rhythmManager.textEffectPool.Peek().activeSelf)
            {
                current = Instantiate(damageTextObj, textPool);
            }
            else
            {
                current = rhythmManager.textEffectPool.Dequeue();
            }
        }

        current.SetActive(true);
        TextEffect currentEffect = current.GetComponent<TextEffect>();
        {
            currentEffect.text.text = $"- {(GameManager.Instance.Atk / 20).ToString("N2")}";
            Sequence seq = DOTween.Sequence();
            seq.OnStart(() => {
                currentEffect.transform.position = Camera.main.WorldToScreenPoint(bossTextEffectPos.position);
            });
            seq.Append(currentEffect.GetComponent<RectTransform>().DOAnchorPosY(current.transform.position.y + 50, 2).SetEase(Ease.InBack));
            seq.Join(currentEffect.text.DOFade(0, 2));
            seq.OnComplete(() => currentEffect.gameObject.SetActive(false));
        }

        damageTextPool.Enqueue(current);

    }

    void MakePlayerAttackEffect()
    {
        GameObject current;
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

        if(current.GetComponent<PlayerAttackEffectObj>().weaponAttackPos == null)
        {
            current.GetComponent<PlayerAttackEffectObj>().weaponAttackPos = playerAttackPos;
        }

        current.SetActive(true);
        current.transform.position = playerAttackPos.position;
        attackEffectPool.Enqueue(current);
    }

    void MakePlayerSoundEffect()
    {
        GameObject current;
        if (attackSoundEffectPool.Count > 0)
        {
            if (attackEffectPool.Peek().activeSelf)
            {
                current = Instantiate(soundObj, attackPool);
            }
            else
            {
                current = attackSoundEffectPool.Dequeue();
            }
        }
        else
        {
            current = Instantiate(soundObj, attackPool);
        }

        current.SetActive(false);

        current.GetComponent<AudioSource>().clip = audioClips[Random.Range(0, audioClips.Length)];
        current.SetActive(true);
        current.transform.position = playerAttackPos.position;
        attackSoundEffectPool.Enqueue(current);
        soundEffectPool.Enqueue(current);
        Invoke("SetActFalseForInvoke", 1f);
    }

    void SetActFalseForInvoke()
    {
        soundEffectPool.Dequeue().SetActive(false);
    }

    void FightSequnce()
    {
        playerAnimator.gameObject.transform.DOKill();
        bossAnimator.gameObject.transform.DOKill();
        cameraPos.DOComplete();
        MakeBossDamageRate();
        Sequence seq = DOTween.Sequence();

        seq.OnStart(() => isTime = false);

        seq.Append(playerAnimator.gameObject.transform.DOMove(new Vector2(-1f, -3), 0.1f).OnComplete(() =>
        {
            playerAnimator.gameObject.transform.DOMove(currentPlayerPos, 0.5f);
        }));

        seq.Join(bossAnimator.gameObject.transform.DOMove(new Vector2(0.2f, -4), 0.1f).OnComplete(() =>
        {
            bossAnimator.SetTrigger(bossHitHash);
            MakePlayerAttackEffect();
            MakePlayerSoundEffect();
            bossAnimator.gameObject.transform.DOMove(currentBossPos, 0.5f);
        }));

        seq.Join(cameraPos.DOShakePosition(cameraShakeTime, cameraShakePower).OnComplete(() => cameraPos.position = new Vector3(0, -1.5f, -10)));

        seq.OnComplete(() => isTime = true);
    }


    private void FinishClicker(bool isClear)
    {
        isActiveCliker = false;
        bgPanel.SetActive(true);

        Text[] texts = new Text[4]; // 0번은 클리어 메세지 1번은 등급 2번은 보상 텍스트 3번은 버튼에 붙은 텍스트
        for (int i = 0; i < 3; i++)
        {
            texts[i] = resultPanel.GetComponentsInChildren<Text>()[i];
        }

        Sequence seq = DOTween.Sequence();

        if (isClear)
        {
            seq.Append(
            texts[0].DOText($"보스 클리어에 성공했습니다!\n리듬게임 정확도 {rhythmManager.GetRatePercent().ToString("N3")} %", 3f).OnComplete(() =>
            {
                if (rhythmManager.GetRatePercent() >= 99.99f)
                {
                    texts[1].text = "P";
                }
                if (rhythmManager.GetRatePercent() > 95f)
                {
                    texts[1].text = "A +";
                }
                else if (rhythmManager.GetRatePercent() > 90f)
                {
                    texts[1].text = "A";
                }
                else if (rhythmManager.GetRatePercent() > 80f)
                {
                    texts[1].text = "B";
                }
                else if (rhythmManager.GetRatePercent() > 70f)
                {
                    texts[1].text = "C";
                }
                else
                {
                    texts[1].text = "F";
                }
            }));

            seq.Append(texts[2].DOText("보상 : 공격력 10% 증가", 0.3f)).OnComplete(() => btnChangeScene.onClick.AddListener(() => GameManager.Instance.ChangeSceneToMainScene(isClear)));
        }
        else
        {
            seq.Append(
            texts[0].DOText($"보스 클리어에 실패했습니다..\n리듬게임 정확도 {rhythmManager.GetRatePercent().ToString("N3")} %", 3f).OnComplete(() =>
            {
                if (rhythmManager.GetRatePercent() >= 100f)
                {
                    texts[1].text = "P";
                }
                if (rhythmManager.GetRatePercent() > 95f)
                {
                    texts[1].text = "A +";
                }
                else if (rhythmManager.GetRatePercent() > 90f)
                {
                    texts[1].text = "A";
                }
                else if (rhythmManager.GetRatePercent() > 80f)
                {
                    texts[1].text = "B";
                }
                else if (rhythmManager.GetRatePercent() > 70f)
                {
                    texts[1].text = "C";
                }
                else
                {
                    texts[1].text = "F";
                }
            }));
            seq.Append(texts[2].DOText("", 0f)).OnComplete(() => btnChangeScene.onClick.AddListener(() => GameManager.Instance.ChangeSceneToMainScene(isClear)));
        }



        resultPanel.SetActive(true);
        clickerText.gameObject.SetActive(false);
    }
}
