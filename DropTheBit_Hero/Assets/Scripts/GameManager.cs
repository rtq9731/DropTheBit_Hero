using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Boss bossSheet;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapon weaponSheet;
    [SerializeField] Work workSheet;

    #region Getter와 변수

    private ushort currentBossIndex = 0;

    public OsuParser parsingManager = null;
    private RhythmManager rhythmManager;

    private List<string> weaponNames = new List<string>();
    private List<string> workNames = new List<string>();

    private long money = 0;
    private int killCount = 0;

    public int isFinishParshing;

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
    public int Combo { get { return combo; } set { combo = value; } }
    public AudioClip GetMusic()
    {
        return Resources.Load($"SongMP3/{bossSheet.dataArray[nowEnemyIndex].Songname}") as AudioClip;
    }
    public decimal GetMoney()
    {
        return money;
    }
    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value;
            MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
            SaveData();

            if (killCount - 5 == 0)
            {
                Debug.LogError("-10해서 보스 바꿔주게 하세욧!");
                MainSceneManager.Instance.CallBoss();
            }

            if (killCount - 10 * (currentBossIndex + 1) <= 0 && bossSheet.dataArray[currentBossIndex].Iscleared)
            {
                bossSheet.dataArray[currentBossIndex].Iscleared = true;
                currentBossIndex++;
            }

            if (killCount - 10 * (nowEnemyIndex + 1) >= 0 && killCount < enemyNames.Count * 10)
            {
                nowEnemyIndex++;
            }

        }
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
        return parsingManager.BeatmapData[bossSheet.dataArray[currentBossIndex].Songname];
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
        parsingManager.Parsing(bossSheet.dataArray[currentBossIndex].Songname);
        yield return new WaitForSeconds(0.5f);
        DG.Tweening.DOTween.Clear();
    }
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
        PlayerPrefs.SetString("Money", money.ToString());
        PlayerPrefs.SetInt("KillCount", killCount);
        PlayerPrefs.SetFloat("ATK", MainSceneManager.Instance.Player.ATK);
        PlayerPrefs.SetInt("EnemyIndex", nowEnemyIndex);
        PlayerPrefs.Save();
#if UNITY_EDITOR
        Debug.Log("Saved!");
#endif
    }

    public void LoadData()
    {
        long.TryParse(PlayerPrefs.GetString("Money"), out money);
        killCount = PlayerPrefs.GetInt("KillCount");
        nowEnemyIndex = PlayerPrefs.GetInt("EnemyIndex");
        MainSceneManager.Instance.Player.ATK = PlayerPrefs.GetFloat("ATK", 20);
#if UNITY_EDITOR
        Debug.Log("Loaded!");
#endif
    }

    #endregion

    void Start()
    {
        InputCommonEnemyData();
        InputWorkData();
        InputWeponData();
        LoadData();
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
        MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
    }

    public void AddMoney(long money)
    {
        this.money += money;
        MainSceneManager.Instance.PlayMoneyEffect(money);
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
    }
}
