using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    [Header("��ü ���ٿ�")]
    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapons weaponSheet;

    [Header("�ٸ� ������Ʈ ���ٿ�")]
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;

    private Dictionary<string, MonsterData> monsters = new Dictionary<string, MonsterData>();
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();
    public List<string> weaponNames = new List<string>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;
        InputCommonEnemyData();
        InputWeponData();
    }
    void OnDestroy()
    {
        Instance = null;
    }

    public Dictionary<string, WeaponsData> GetWeponDictionary()
    {
        return weapons;
    }

    private void Start()
    {
        enemyPrefabs.Add("Skeleton", Resources.Load("Enemies/Skeleton") as GameObject);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void CallNextEnmey()
    {
        GameObject temp = enemyPrefabs["Skeleton"];
        Instantiate(temp, new Vector2(8, 1), Quaternion.identity);
    }

    public void ScrollingBackground()
    {
        backGround.isScroll = !backGround.isScroll;
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
}
