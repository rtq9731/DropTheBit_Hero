using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapon weaponSheet;
    [SerializeField] Work workSheet;
    [SerializeField] AudioClip[] songs;

    public BeatMapScriptableObj beatMap;

    #region Getter와 변수

    DataSaveClass saveData;
    float atk;
    string filePath;
    private ushort currentBossIndex = 0;

    public OsuParser parsingManager = null;
    private RhythmManager rhythmManager;

    private List<string> weaponNames = new List<string>();
    private List<string> workNames = new List<string>();

    private long money = 0;
    private int killCount = 0;

    private int combo = 0;
    int nowEnemyIndex = 0;

    private List<string> enemyNames = new List<string>();

    private Dictionary<string, MonsterData> enemyDatas = new Dictionary<string, MonsterData>();
    private Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>();
    private Dictionary<string, WorkData> works = new Dictionary<string, WorkData>();

    public Dictionary<string, WorkData> Works { get { return works; } }
    public Dictionary<string, WeaponData> Weapons { get { return weapons; } }
    public int NowEnemyIndex { get { return nowEnemyIndex; } }
    public Dictionary<string, MonsterData> EnemyDatas { get { return enemyDatas; } }
    public List<string> EnemyNames { get { return enemyNames; } }
    public float Atk { get { return atk; } }
    public int Combo { get { return combo; } set { combo = value; } }
    public AudioClip GetMusic()
    {
        return songs[currentBossIndex];
    }
    public decimal GetMoney()
    {
        return money;
    }

    public void UPBossCount()
    {
        currentBossIndex++;
    }

    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value;
            MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
            SaveData();

            if ((currentBossIndex + 1) * 10 - killCount <= 0)
            {
                MainSceneManager.Instance.CallBoss();
            }

            if (killCount - 10 * (currentBossIndex + 1) >= 0 && beatMap.beatmaps[currentBossIndex].isCleared)
            {
                if (beatMap.beatmaps[currentBossIndex].isCleared)
                    currentBossIndex++;
            }
        }
    }
    #endregion

    #region 유니티 메세지

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/SaveData.txt";
#if UNITY_EDITOR
        Debug.Log(filePath);
#endif
    }

    void Start()
    {
        beatMap = Resources.Load("Notes/BeatMap_Data") as BeatMapScriptableObj;
        InputCommonEnemyData();
        InputWorkData();
        InputWeponData();
        LoadData();
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
        MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
    }

    #endregion

    #region 일반 메소드

    public WeaponData GetWeaponByIndex(int index)
    {
        return weapons[weaponNames[index]];
    }

    public WorkData GetWorkDataByindex(int index)
    {
        return works[workNames[index]];
    }

    public Beatmap GetCurrentBeatmap()
    {
        return beatMap.beatmaps[currentBossIndex];
    }

    public BeatMapScriptableObj.VirtualBeatmaps GetVirtualBeatmaps()
    {
        return beatMap.virtualBeatmaps[currentBossIndex];
    }

    public void AddMoney(long money)
    {
        this.money += money;
        MainSceneManager.Instance.PlayMoneyEffect(money);
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
    }

    #endregion

    #region Scene 관련

    public void ChangeSceneToBossScene()
    {
        StartCoroutine(ChangeSceneToBoss());
    }

    public void ChangeSceneToMainScene(bool isCleared)
    {
        StartCoroutine(ChangeSceneToMain(isCleared));
    }

    public void ParsingSongs()
    {
        for (int i = 0; i < songs.Length; i++)
        {
            parsingManager.Parsing(songs[i].name);
        }
        beatMap.SetVirtualList();
    }

    private IEnumerator ChangeSceneToBoss()
    {
        DG.Tweening.DOTween.Clear();
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
        SceneManager.LoadScene("BossScene");
        while (SceneManager.GetActiveScene().name != "BossScene")
        {
            yield return null;
        }
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
        yield return new WaitForSeconds(0.5f);
        DG.Tweening.DOTween.Clear();
    }
    private IEnumerator ChangeSceneToMain(bool isCleared)
    {
        DG.Tweening.DOTween.Clear();
        SceneManager.LoadScene("MainScene");
        while (SceneManager.GetActiveScene().name != "MainScene")
        {
            yield return null;
        }
        //bossSheet.dataArray[currentBossIndex].Iscleared = isCleared;
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.SetResolution(1440, 2560, Screen.fullScreen);
    }
    #endregion

    #region 데이터 초기화 메소드
    private void InputCommonEnemyData()
    {
        for (int i = 0; i < enemySheet.dataArray.Length; i++)
        {
            enemyDatas.Add(enemySheet.dataArray[i].Name, enemySheet.dataArray[i]);
            enemyNames.Add(enemySheet.dataArray[i].Name);
        }
    }

    private void InputWorkData()
    {
        for (int i = 0; i < weaponSheet.dataArray.Length; i++)
        {
            works.Add(workSheet.dataArray[i].Name, workSheet.dataArray[i]);
            workNames.Add(workSheet.dataArray[i].Name);
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
    #endregion

    #region 저장과 불러오기

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        weapons = new Dictionary<string, WeaponData>();
    }

    public void SaveData()
    {
        if(saveData == null)
        {
            saveData = new DataSaveClass(weapons, works, money, killCount, nowEnemyIndex, MainSceneManager.Instance.Player.ATK, currentBossIndex);
        }
        else
        {
            saveData.InitSaveClass(weapons, works, money, killCount, nowEnemyIndex, MainSceneManager.Instance.Player.ATK);
        }
        string jsonString = JsonUtility.ToJson(saveData);
        FileStream fs = new FileStream(filePath, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonString);
        fs.Write(data, 0, data.Length);
        fs.Close();
        LoadData();
    }

    public void LoadData()
    {
        if (!File.Exists(filePath))
        {
            SaveData();
        }

        FileStream fs = new FileStream(filePath, FileMode.Open);
        byte[] data = new byte[fs.Length];
        fs.Read(data, 0, data.Length);
        fs.Close();
        string jsonString = Encoding.UTF8.GetString(data);
        saveData = JsonUtility.FromJson<DataSaveClass>(jsonString);

        atk = 20;
        saveData.loadData(out weapons, out works, out money, out killCount, out nowEnemyIndex, out atk, out currentBossIndex);
        MainSceneManager.Instance.Player.ATK = atk;
    }

    #endregion
}
