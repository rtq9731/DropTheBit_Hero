using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] public Notes noteSheet;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapons weaponSheet;

    #region Getter와 변수

    [SerializeField] public string nowPlaySong = "";

    public OsuParser parsingManager = null;
    private RhythmManager rhythmManager;

    public List<string> weaponNames = new List<string>();

    private long money = 0;
    private int killCount = 0;

    public int isFinishParshing;

    private int combo = 0;
    int nowEnemyIndex = 0;

    private List<string> enemyNames = new List<string>();

    private Dictionary<string, MonsterData> enemyDatas = new Dictionary<string, MonsterData>();
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();

    public Dictionary<string, WeaponsData> Weapons { get { return weapons; } }
    public int NowEnemyIndex { get { return nowEnemyIndex; } }
    public Dictionary<string, MonsterData> EnemyDatas { get { return enemyDatas; } }
    public List<string> EnemyNames { get { return enemyNames; } }
    public int Combo { get { return combo; } set { combo = value; } }
    public AudioClip GetMusic()
    {
        return Resources.Load($"SongMP3/{nowPlaySong}") as AudioClip;
    }
    public decimal GetMoney()
    {
        return money;
    }
    private void GetRhythmManager()
    {
        rhythmManager = FindObjectOfType<RhythmManager>();
        rhythmManager.parsingSongName = nowPlaySong;
    }
    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value;
            MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
            SaveData();

            if (killCount % 3 == 0)
            {
                MainSceneManager.Instance.CallBoss();
            }

            if (killCount % 5 == 0 && killCount < enemyNames.Count * 5)
            {
                nowEnemyIndex++;
            }

        }
    }
    #endregion

    #region 일반 메소드

    public WeaponsData GetWeaponByIndex(int index)
    {
        return weapons[weaponNames[index]];
    }
    #endregion

    #region Scene 관련

    public void ChangeSceneToBossScene()
    {
        StartCoroutine(ChangeSceneToBoss());
    }

    public void ChangeSceneToMainScene()
    {
        StartCoroutine(ChangeSceneToMain());
    }

    private IEnumerator ChangeSceneToBoss()
    {
        DG.Tweening.DOTween.Clear();
        SceneManager.LoadScene("BossScene");
        while (SceneManager.GetActiveScene().name != "BossScene")
        {
            yield return null;
        }
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
        parsingManager.Parsing();
        yield return new WaitForSeconds(0.5f);
        GetRhythmManager();
        DG.Tweening.DOTween.Clear();
    }
    #endregion

    #region 코루틴
    private IEnumerator ChangeSceneToMain()
    {
        DG.Tweening.DOTween.Clear();
        SceneManager.LoadScene("MainScene");
        while (SceneManager.GetActiveScene().name != "MainScene")
        {
            yield return null;
        }
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
        weapons = new Dictionary<string, WeaponsData>();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("Money", money.ToString());
        PlayerPrefs.SetInt("KillCount", killCount);
        PlayerPrefs.SetFloat("ATK", MainSceneManager.Instance.Player.ATK);
        PlayerPrefs.SetInt("EnemyIndex", nowEnemyIndex);
        PlayerPrefs.Save();
        Debug.Log("Saved!");
    }

    public void LoadData()
    {
        long.TryParse(PlayerPrefs.GetString("Money"), out money);
        killCount = PlayerPrefs.GetInt("KillCount");
        nowEnemyIndex = PlayerPrefs.GetInt("EnemyIndex");
        MainSceneManager.Instance.Player.ATK = PlayerPrefs.GetFloat("ATK", 20);
        Debug.Log("Loaded!");
    }

    #endregion

    void Awake()
    {
        InputCommonEnemyData();
    }

    private void Start()
    {
        LoadData();
        InputWeponData();
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
        MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
    }

    public void AddMoney(int money)
    {
        this.money += money;
        MainSceneManager.Instance.PlayMoneyEffect(money);
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
    }
}
