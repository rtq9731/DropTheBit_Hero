using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Notes noteSheet;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapons weaponSheet;

    public List<string> weaponNames = new List<string>();
    private Dictionary<string, MonsterData> monsters = new Dictionary<string, MonsterData>();
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, List<float>> noteListDictionary = new Dictionary<string, List<float>>();


    private int money = 0;
    private int killCount = 0;
    private float stage = 0;
    private int combo = 0;

    public int KillCount { get { return killCount; } set { killCount = value; MainSceneManager.Instance.topUI.UpdateCurrentKillCount(); } }

    public Dictionary<string, WeaponsData> GetWeponDictionary()
    {
        return weapons;
    }

    void Awake()
    {
        LoadAllSongs();
        InputCommonEnemyData();
        InputWeponData();
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

    public string OutRandomSongName()
    {
        return noteSheet.dataArray[Random.Range(0, noteSheet.dataArray.Length)].Songname;
    }

    private void LoadAllSongs()
    {
        for (int i = 0; i < noteSheet.dataArray.Length; i++)
        {
            List<float> noteList = new List<float>();
            string[] tempStringList1 = (Resources.Load(noteSheet.dataArray[i].Datapath) as TextAsset).text.Split('\n');
            for (int j = 0; j < tempStringList1.Length; j++)
            {
                string[] tempStringList2 = tempStringList1[j].Split('|'); // 줄마다 잘라서 가져온 tempStringList1을 | 을 기준으로 잘라서 tempStringList2에 넣어줌

                if (tempStringList2[0] == null || tempStringList2[0] == "") // 만약 잘랐는데 아무것도 없으면 반복 끝
                {
                    break;
                }

                string timestring = tempStringList2[0].Split(',')[2]; // | 기준으로 잘린 tempStirngList2 의 0번을 , 기준으로 잘랐을 때, 2번째 인덱스에 시간 정보가 담겨있음 
                string realTimeString = ""; // 정수 형태의 시간
                string floatTimeString = ""; // 정수를 제외한 시간 ( 소수 시간 )
                for (int k = 0; k < timestring.Length; k++)
                {
                    if(k < timestring.Length - 3) // 뒤에서 3자리 1.000 -> 000 부분을 제외한 나머지
                    {
                        realTimeString += timestring[k]; // 정수형 시간에 추가해줌
                    }
                    else
                    {
                        floatTimeString += timestring[k]; // 소수 시간에 추가해줌
                    }
                }
                realTimeString += '.'; // 소수를 넣기 전 먼저 . 을 넣어줘 소숫점을 구분해줌
                realTimeString += floatTimeString; // 소수를 넣음
                if(float.TryParse(realTimeString, out float timeFloat)) // TryParse를 통해 소수와 정수가 합쳐진 realTimeString을 float형으로 형변환
                {
                    noteList.Add(timeFloat);
                }
            }
            noteListDictionary.Add(noteSheet.dataArray[i].Songname, noteList); // 마지막으로 NoteListDictionalry에 noteList를 넣어줘 곡 이름으로 노트들이 저장될 수 있게 함.
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
