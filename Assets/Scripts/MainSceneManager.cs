using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] Transform enemyPoolTr;
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;
    [SerializeField] public AudioSource hitSound;
    [SerializeField] public InputPanel InputPanel;
    [SerializeField] public LeftUI leftUI;

    public List<GameObject> enemyPool = new List<GameObject>();

    public Player Player { get { return player; } set { player = value; } }

    private void Start()
    {
        CallNextEnemy();
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
                MonsterData data = GameManager.Instance.EnemyDatas[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]];
                item.GetComponent<Enemy>().InitEnemy(data.Cost, data.HP);
                item.GetComponent<Transform>().position = new Vector2(8, 1);
                item.SetActive(true);
                return;
            }
        }

        MakeNewEnemyInPool();
    }

    private void MakeNewEnemyInPool()
    {
        GameObject temp = Instantiate(GameManager.Instance.EnemyPrefab, new Vector2(8, 1), Quaternion.identity, enemyPoolTr);
        MonsterData data = GameManager.Instance.EnemyDatas[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]];
        temp.GetComponent<Enemy>().InitEnemy(data.Cost, data.HP);
        temp.GetComponent<Transform>().position = new Vector2(8, 1);
        enemyPool.Add(temp);
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
