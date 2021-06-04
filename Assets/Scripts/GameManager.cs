using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Notes noteSheet;

    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapons weaponSheet;
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;
    [SerializeField] List<AudioClip> songList;


    public List<string> weaponNames = new List<string>();

    private Dictionary<string, MonsterData> monsters = new Dictionary<string, MonsterData>();
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();
    private List<float> noteList = new List<float>();
    private Dictionary<string, List<float>> noteListDictionary = new Dictionary<string, List<float>>();


    private int money;
    private int killCount;
    private float stage;
    private int combo;

    public int KillCount { get { return killCount; } set { killCount = value; topUI.UpdateCurrentKillCount(); } }

    public Dictionary<string, WeaponsData> GetWeponDictionary()
    {
        return weapons;
    }

    void Awake()
    {
        LoadAllSongs();
        InputCommonEnemyData();
        InputWeponData();

        foreach (var item in noteListDictionary)
        {
            Debug.Log(item.Key);
        }
    }

    private void Start()
    {
        enemyPrefabs.Add("Skeleton", Resources.Load("Enemies/Skeleton") as GameObject);
    }
    public void AddCombo(int num)
    {
        combo += num;
    }

    public void BreakCombo()
    {
        combo = 0;
    }

    public List<float> GetSongNotesByName(string songName)
    {
        return noteListDictionary[songName];
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

    public void AddMoney(int addNum)
    {
        money += addNum;
        topUI.UpdateCurrentCoin();
    }

    public int GetMoney()
    {
        return money;
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

    public string OutRandomSongName()
    {
        return noteSheet.dataArray[Random.Range(0, noteSheet.dataArray.Length)].Songname;
    }

    private void LoadAllSongs()
    {
        for (int i = 0; i < noteSheet.dataArray.Length; i++)
        {
            string[] tempStringList1;
            tempStringList1 = noteSheet.dataArray[i].Hitobjects.Split('\n');
            for (int j = 0; j < tempStringList1.Length; j++)
            {
                string[] tempStringList2;
                tempStringList2 = tempStringList1[j].Split('|');
                string timestring = tempStringList2[0].Split(',')[2];
                string realTimeString = "";
                string floatTimeString = "";
                for (int k = 0; k < timestring.Length; k++)
                {
                    if(k < timestring.Length - 3) // 뒤에서 3자리 1.000 -> 000 부분을 제외한 나머지
                    {
                        realTimeString += timestring[k];
                    }
                    else
                    {
                        floatTimeString += timestring[k];
                    }
                }
                realTimeString += '.';
                realTimeString += floatTimeString;
                if(float.TryParse(realTimeString, out float timeFloat))
                {
                    noteList.Add(timeFloat);
                }
            }
            noteListDictionary.Add(noteSheet.dataArray[i].Songname, noteList);
        }
    }
}
