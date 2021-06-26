using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        Instance = null;
    }

    [SerializeField] GameObject enemyPrefab;

    [Header("이펙트 관련")]
    [SerializeField] Transform moneyEffectTr;
    [SerializeField] GameObject moneyEffectPrefab;
    [SerializeField] GameObject enemyNameTextPanel;
    [SerializeField] Animator effectAnimator;
    [SerializeField] Slider enemyHpSlider;

    [Header("배경")]
    [SerializeField] BackGroundMove backGround;

    [Header("상단 UI")]
    [SerializeField] Text atkText = null;

    [Header("풀링용")]
    [SerializeField] Transform enemyPoolTr;
    [SerializeField] Transform moneyEffectPoolTr;

    [Header("여타 참조용 Public 변수들")]
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;
    [SerializeField] public WorkUI workUI;
    [SerializeField] public AudioSource hitSound;
    [SerializeField] public InputPanel InputPanel;
    [SerializeField] public LeftUI leftUI;
    [SerializeField] Player player;

    Enemy nowEnemy;
    Text enemyNameText;

    public List<GameObject> enemyPool = new List<GameObject>();
    public Queue<GameObject> moneyEffectPool = new Queue<GameObject>();

    public Player Player { get { return player; } set { player = value; } }

    private void Start()
    {
        CallNextEnemy();
        effectAnimator.speed = 0;
    }

    private void Update()
    {
        atkText.text = $"공격력\n{player.ATK}";

#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            GameManager.Instance.ClearData();
        }

        if(Input.GetKeyDown(KeyCode.Insert))
        {
            GameManager.Instance.AddMoney(100000);
        }
#endif

        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }

        if (nowEnemy != null)
        {
            enemyHpSlider.transform.position = nowEnemy.hpBarPos.position;
            enemyNameTextPanel.transform.position = nowEnemy.namePanelPos.position;
            enemyNameText.text = GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex];
            effectAnimator.GetComponentInParent<Transform>().position = nowEnemy.effectPos.position;
        }
    }

    public void UpdateHPSlider()
    {
        enemyHpSlider.value = nowEnemy.hp;
    }

    public void PlayAttackEffect()
    {
        effectAnimator.speed = 1;
        effectAnimator.SetTrigger($"SlashEffect");
    }

    public void StopAttackEffect()
    {
        effectAnimator.SetTrigger("FastStop");
    }

    public void CallBoss()
    {
        leftUI.SetActiveTrueBtnBoss();
    }

    public void CallNextEnemy()
    {
        foreach (var item in enemyPool)
        {
            if(!item.activeSelf) // 만약 풀 안에 ActiveFalse된 오브젝트가 있다면.
            {
                //StopAttackEffect();
                MonsterData data = GameManager.Instance.EnemyDatas[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]];
                nowEnemy = item.GetComponent<Enemy>();


                enemyNameText = enemyNameTextPanel.GetComponentInChildren<Text>();
                nowEnemy.InitEnemy(data.Cost, data.HP);
                enemyNameText.text = GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex];
                enemyHpSlider.maxValue = nowEnemy.hp;
                enemyHpSlider.value = nowEnemy.hp;

                item.SetActive(true);

                item.GetComponent<Transform>().position = new Vector2(8, 1);
                return;
            }
        }

        //StopAttackEffect();
        MakeNewEnemyInPool();
    }

    private void MakeNewEnemyInPool()
    {
        GameObject temp = Instantiate(enemyPrefab, new Vector2(8, 1), Quaternion.identity, enemyPoolTr);
        temp.SetActive(false);
        MonsterData data = GameManager.Instance.EnemyDatas[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]];
        nowEnemy = temp.GetComponent<Enemy>();
        nowEnemy.transform.position = new Vector2(8, 1);

        enemyNameText = enemyNameTextPanel.GetComponentInChildren<Text>();
        enemyNameText.text = nowEnemy.name.Split('(')[0];

        nowEnemy.InitEnemy(data.Cost, data.HP);
        enemyHpSlider.maxValue = nowEnemy.hp;
        enemyHpSlider.value = nowEnemy.hp;

        enemyPool.Add(temp);

        temp.SetActive(true);
    }

    public void PlayMoneyEffect(long cost)
    {
        if(moneyEffectPool.Count <= 0)
        {
            CreateMoneyEffect(cost);
        }
        else
        {
            if (moneyEffectPool.Peek().activeSelf != false)
            {
                CreateMoneyEffect(cost);
            }
            else
            {
                InitMoneyEffect(cost);
            }
        }
    }

    void InitMoneyEffect(long cost)
    {
        hitSound.Play();
        GameObject moneyText = moneyEffectPool.Dequeue();
        Text text = moneyText.GetComponentInChildren<Text>();
        moneyText.GetComponentsInChildren<Image>()[1].color = Color.white;
        moneyText.GetComponentInChildren<Text>().color = Color.white;

        text.text = cost > 0 ? text.text = $"+ {cost}" : text.text = $"- {-cost}";
        moneyText.transform.position = new Vector2(moneyEffectTr.position.x + 1f, moneyEffectTr.position.y - 0.2f);
        moneyText.gameObject.SetActive(true);
        moneyText.transform.DOMoveY(moneyEffectTr.position.y, 1f);
        moneyText.GetComponentsInChildren<Image>()[1].DOFade(0, 1f);
        moneyText.GetComponentInChildren<Text>().DOFade(0, 1f).OnComplete(() => moneyText.transform.gameObject.SetActive(false));
        moneyEffectPool.Enqueue(moneyText.gameObject);
    }

    void CreateMoneyEffect(long cost)
    {
        var temp = Instantiate(moneyEffectPrefab, moneyEffectPoolTr);
        temp.SetActive(false);
        moneyEffectPool.Enqueue(temp);
        InitMoneyEffect(cost);
    }

    public void SceneChange()
    {
        GameManager.Instance.ChangeSceneToBossScene();
    }

    public void ScrollingBackground()
    {
        backGround.isScroll = !backGround.isScroll;
    }

    public void DoHit()
    {
        hitSound.Play();
    }
}
