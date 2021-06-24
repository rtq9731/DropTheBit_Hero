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

    [Header("AnimatorController들")]
    [SerializeField] RuntimeAnimatorController skeletonController;

    [Header("기타")]
    [SerializeField] GameObject moneyEffectPrefab;
    [SerializeField] Animator effectAnimator;
    [SerializeField] GameObject enemyNameTextPanel;
    [SerializeField] Slider enemyHpSlider;
    [SerializeField] BackGroundMove backGround;
    [SerializeField] Text atkText = null;
    [SerializeField] Player player;
    [SerializeField] Transform enemyPoolTr;
    [SerializeField] Transform moneyEffectPoolTr;
    [SerializeField] Transform moneyEffectTr;
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;
    [SerializeField] public AudioSource hitSound;
    [SerializeField] public InputPanel InputPanel;
    [SerializeField] public LeftUI leftUI;

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
        Debug.Log(Camera.main.ScreenToWorldPoint(nowEnemy.gameObject.transform.position));
        atkText.text = $"공격력\n{player.ATK}";

        if (nowEnemy != null)
        {
            enemyHpSlider.transform.position = new Vector2(nowEnemy.transform.position.x, nowEnemy.transform.position.y - 0.2f);
            enemyNameTextPanel.transform.position = new Vector2(nowEnemy.transform.position.x, nowEnemy.transform.position.y + 1f);
            enemyNameText.text = GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex];
            effectAnimator.GetComponentInParent<Transform>().position = new Vector2(nowEnemy.transform.position.x - 0.45f, nowEnemy.transform.position.y + 0.5f);
        }
    }

    public void UpdateHPSlider()
    {
        enemyHpSlider.value = nowEnemy.hp;
    }

    public void PlayAttackEffect()
    {
        effectAnimator.speed = 1;
        effectAnimator.SetTrigger($"Effect{GameManager.Instance.WeaponeIndex}");
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
                item.GetComponent<Enemy>().InitEnemy(skeletonController, data.Cost, data.HP);
                nowEnemy = item.GetComponent<Enemy>();
                enemyHpSlider.transform.position = new Vector2(nowEnemy.transform.position.x, nowEnemy.transform.position.y - 0.2f);
                enemyHpSlider.maxValue = item.GetComponent<Enemy>().hp;
                enemyHpSlider.value = item.GetComponent<Enemy>().hp;

                enemyNameText = enemyNameTextPanel.GetComponentInChildren<Text>();
                enemyNameTextPanel.transform.position = new Vector2(nowEnemy.transform.position.x, nowEnemy.transform.position.y + 2f);
                enemyNameText.text = GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex];
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
        GameObject temp = Instantiate(GameManager.Instance.EnemyPrefab, new Vector2(8, 1), Quaternion.identity, enemyPoolTr);
        MonsterData data = GameManager.Instance.EnemyDatas[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]];
        temp.GetComponent<Enemy>().InitEnemy(skeletonController, data.Cost, data.HP);
        temp.GetComponent<Transform>().position = new Vector2(8, 1);
        nowEnemy = temp.GetComponent<Enemy>();
        enemyHpSlider.transform.position = new Vector2(nowEnemy.transform.position.x, nowEnemy.transform.position.y - 0.2f);
        enemyHpSlider.maxValue = nowEnemy.hp;
        enemyHpSlider.value = nowEnemy.hp;

        enemyNameText = enemyNameTextPanel.GetComponentInChildren<Text>();
        enemyNameTextPanel.transform.position = new Vector2(nowEnemy.transform.position.x, nowEnemy.transform.position.y + 2f);
        enemyNameText.text = nowEnemy.name.Split('(')[0];
        enemyPool.Add(temp);
    }

    public void PlayMoneyEffect(int cost)
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

    void InitMoneyEffect(int cost)
    {
        hitSound.Play();
        GameObject moneyText = moneyEffectPool.Dequeue();
        Text text = moneyText.GetComponentInChildren<Text>();
        moneyText.GetComponentsInChildren<Image>()[1].color = Color.white;
        moneyText.GetComponentInChildren<Text>().color = Color.white;

        text.text = cost > 0 ?
        text.text = $"+ {cost}" : text.text = $"- {-cost}";
        Debug.Log(moneyText.GetComponentInParent<Transform>().gameObject.name);
        moneyText.transform.position = new Vector2(moneyEffectTr.position.x + 1f, moneyEffectTr.position.y - 0.2f);
        moneyText.gameObject.SetActive(true);
        moneyText.transform.DOMoveY(moneyEffectTr.position.y, 1f);
        moneyText.GetComponentsInChildren<Image>()[1].DOFade(0, 1f);
        moneyText.GetComponentInChildren<Text>().DOFade(0, 1f).OnComplete(() => moneyText.transform.gameObject.SetActive(false));
        moneyEffectPool.Enqueue(moneyText.gameObject);
    }

    void CreateMoneyEffect(int cost)
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
