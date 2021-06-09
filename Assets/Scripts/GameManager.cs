using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Notes noteSheet;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapons weaponSheet;
    [SerializeField] OsuParser parsingManager = null;

    public List<string> weaponNames = new List<string>();
    private Dictionary<string, MonsterData> monsters = new Dictionary<string, MonsterData>();
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();


    private int money = 0;
    private int killCount = 0;

    public int KillCount { get { return killCount; } set { killCount = value; MainSceneManager.Instance.topUI.UpdateCurrentKillCount(); } }

    public Dictionary<string, WeaponsData> GetWeponDictionary()
    {
        return weapons;
    }

    void Awake()
    {
        InputCommonEnemyData();
        InputWeponData();
    }

    private void Start()
    {
        enemyPrefabs.Add("Skeleton", Resources.Load("Enemies/Skeleton") as GameObject);
    }

    public void CallNextEnmey(string nameKey)
    {
        GameObject temp = enemyPrefabs[nameKey];
        Instantiate(temp, new Vector2(8, 1), Quaternion.identity);
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int money)
    {
        this.money += money;
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
    }

    private void InputCommonEnemyData()
    {
        for (int i = 0; i < enemySheet.dataArray.Length; i++)
        {
            monsters.Add(enemySheet.dataArray[i].Name, enemySheet.dataArray[i]);
        }
    }

    private void InputWeponData()
    {
        for (int i = 0; i < weaponSheet.dataArray.Length; i++)
        {
            weapons.Add(weaponSheet.dataArray[i].Name, weaponSheet.dataArray[i]);
            weaponNames.Add(weaponSheet.dataArray[i].Name);
        }
    }


    public void GetEnemyDataFromName(string name, out float hp, out int cost)
    {
        MonsterData tempData = monsters[name];
        if (tempData != null)
        {
            hp = tempData.HP;
            cost = tempData.Cost;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"can't find the Monster by {name}");
#endif
            hp = 0;
            cost = 0;
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
