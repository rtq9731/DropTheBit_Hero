using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] public Notes noteSheet;
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapons weaponSheet;

    SaveClass saveData = new SaveClass();

    #region Getter�� ����
    int nowEnemyIndex = 0;
    public int NowEnemyIndex { get { return nowEnemyIndex; } }

    private Dictionary<string, MonsterData> enemyDatas = new Dictionary<string, MonsterData>();
    public Dictionary<string, MonsterData> EnemyDatas { get { return enemyDatas; } }

    private GameObject enemyPrefab;

    public GameObject EnemyPrefab { get { return enemyPrefab; } }

    private List<string> enemyNames = new List<string>();
    public List<string> EnemyNames { get { return enemyNames; } }
    private int combo = 0;
    public int Combo { get { return combo; } set { combo = value; } }
    #endregion

    [SerializeField] public string nowPlaySong = "";

    public OsuParser parsingManager = null;
    private RhythmManager rhythmManager;

    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();

    public List<string> weaponNames = new List<string>();

    private decimal money = 0;
    private int killCount = 0;

    public int isFinishParshing;
    public int WeaponeIndex;

    public int KillCount { get { return killCount; } 
        set 
        {
            killCount = value;
            MainSceneManager.Instance.topUI.UpdateCurrentKillCount();

            if(killCount % 3 == 0)
            {
                MainSceneManager.Instance.CallBoss();
            }

            if(killCount % 5 == 0 && killCount < enemyNames.Count * 5)
            {
                nowEnemyIndex++;
            }

        } 
    }

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
        PlayerPrefs.SetInt("killCount", killCount);
        PlayerPrefs.SetFloat("ATK", MainSceneManager.Instance.Player.ATK);
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

    private IEnumerator ChangeSceneToMain()
    {
        DG.Tweening.DOTween.Clear();
        SceneManager.LoadScene("MainScene");
        while (SceneManager.GetActiveScene().name != "MainScene")
        {
            yield return null;
        }
        MainSceneManager.Instance.Player.ATK = PlayerPrefs.GetFloat("ATK");
        killCount = PlayerPrefs.GetInt("killCount");
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.SetResolution(1440, 2560, Screen.fullScreen);
    }

    private void GetRhythmManager()
    {
        rhythmManager = FindObjectOfType<RhythmManager>();
        rhythmManager.parsingSongName = nowPlaySong;
    }


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
        enemyPrefab = Resources.Load("Enemies/EnemyPrefab") as GameObject;
    }

    public AudioClip GetMusic()
    {
        return Resources.Load($"SongMP3/{nowPlaySong}") as AudioClip;
    }

    public decimal GetMoney()
    {
        return money;
    }

    public void AddMoney(int money)
    {
        this.money += money;
        MainSceneManager.Instance.PlayMoneyEffect(money);
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
    }

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
}
