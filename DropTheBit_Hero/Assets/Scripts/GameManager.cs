using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Monster enemySheet;
    [SerializeField] Weapon weaponSheet;
    [SerializeField] Work workSheet;
    [SerializeField] GameObject rewardPanelPrefab;
    [SerializeField] GameObject exitPanelPrefab;

    public BeatMapScriptableObj beatMap;

    #region Getter와 변수

    DataSaveClass saveData;
    float atk;
    string filePath;
    private ushort currentBossIndex = 0;

    private bool isStop = false;

    private OsuParser parsingManager = null;
    private RhythmManager rhythmManager;
    private System.DateTime lastconnectTime;
    ExitPanel exitPanel = null;

    private List<string> weaponNames = new List<string>();
    private List<string> workNames = new List<string>();

    List<AudioClip> songs = new List<AudioClip>();

    private long money = 0;
    private int killCount = 0;

    private bool isFirst = true; // -> 여러번 씬 로딩이 안되었는가?
    private bool isFirstPlay = true; // -> 뉴비인가?
    public bool isOpenRewardTap = false; // 리워드 탭이 열려있을땐 esc메뉴가 불러와지면 안되니까

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
    public float Atk { get { return atk; } set { atk = value; } }
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
        Instance.atk += Instance.atk / 10; // 공격력 10% 증가

        if (Instance.currentBossIndex + 1 < Instance.beatMap.beatmaps.Count)
            Instance.PlusBossIndex();
    }

    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value;
            MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
            SaveData();

            if( killCount - (nowEnemyIndex + 1) * 10 > 0 && nowEnemyIndex < enemySheet.dataArray.Length - 1)
            {
                nowEnemyIndex++;
            }

            if ((currentBossIndex + 1) * 50 - killCount <= 0)
            {
                MainSceneManager.Instance.CallBoss();
            }
        }
    }
    #endregion

    #region 유니티 메세지
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/SaveData.txt";

        songs.Add(Resources.Load("Sound_MP3/Nine Point Eight") as AudioClip);
        songs.Add(Resources.Load("Sound_MP3/RTRT") as AudioClip);
        songs.Add(Resources.Load("Sound_MP3/WorldExcuteMe") as AudioClip);

        parsingManager = gameObject.AddComponent<OsuParser>();

#if UNITY_EDITOR
        Debug.Log(filePath);
#endif
    }

    void Start()
    {
        Time.timeScale = 1;
        beatMap = Resources.Load("Notes/BeatMap_Data") as BeatMapScriptableObj;
        InputCommonEnemyData();
        InputWorkData();
        InputWeponData();
        //MainSceneManager.Instance.topUI.UpdateCurrentCoin();
        //MainSceneManager.Instance.topUI.UpdateCurrentKillCount();
    }

    private void Update()
    {
        if (isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            exitPanel.gameObject.transform.DOScale(0, 0.2f).OnComplete(() =>
            {
                exitPanel.gameObject.SetActive(false);
                isStop = false;
            });
        }

        if (!isStop && Input.GetKeyDown(KeyCode.Escape) && !isOpenRewardTap)
        {
            isStop = true;
            Debug.Log(isStop);

            Time.timeScale = 0f;
            if(FindObjectOfType<ExitPanel>() != null)
            {
                exitPanel = FindObjectOfType<ExitPanel>();
            }
            else
            {
                exitPanel = Instantiate(exitPanelPrefab, FindObjectOfType<Canvas>().transform).GetComponent<ExitPanel>();
            }

            exitPanel.gameObject.transform.localScale = Vector3.one;
            exitPanel.gameObject.SetActive(true);
            exitPanel.btnExit.onClick.AddListener(() => Application.Quit());
            exitPanel.btnCancel.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                exitPanel.gameObject.transform.DOScale(0, 0.2f).OnComplete(() =>
                {
                    exitPanel.gameObject.SetActive(false);
                    isStop = false;
                });
            });

            //if(Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Time.timeScale = 1;
            //    exitPanel.gameObject.transform.DOScale(0, 0.2f).OnComplete(() =>
            //    {
            //        exitPanel.gameObject.SetActive(false);
            //        isStop = false;
            //    });
            //}
        }
    }

    #endregion

    #region 일반 메소드

    public void CallGameManager() // 의미 없음. DontDestroyOnLoad에 올리려고 참조.
    { }

    public void PlusBossIndex()
    {
        currentBossIndex++;
    }

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
        for (int i = 0; i < songs.Count; i++)
        {
            parsingManager.Parsing(songs[i].name);
        }
        beatMap.SetVirtualList();
    }

    private IEnumerator ChangeSceneToBoss()
    {
        DOTween.Clear();
        SceneManager.LoadScene("BossScene");
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        while (SceneManager.GetActiveScene().name != "BossScene")
        {
            yield return null;
        }

        Screen.SetResolution(2560, 1440, false);
        yield return new WaitForSeconds(0.5f);
        DOTween.Clear();
    }

    private IEnumerator ChangeSceneToMain(bool isCleared)
    {
        DOTween.Clear();

        SceneManager.LoadScene("MainScene");

        Screen.orientation = ScreenOrientation.Portrait;

        while (SceneManager.GetActiveScene().name != "MainScene")
        {
            yield return null;
        }
        if(isCleared)
            UPBossCount();

        MainSceneManager.Instance.Player.ATK = atk;

        SaveData();

        Screen.SetResolution(1440, 2560, false);
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
        isFirstPlay = false;
        if (saveData == null)
        {
            saveData = new DataSaveClass(weapons, works, money, killCount, nowEnemyIndex, MainSceneManager.Instance.Player.ATK, currentBossIndex, System.DateTime.Now, isFirstPlay);
        }
        else
        {
            saveData.InitSaveClass(weapons, works, money, killCount, nowEnemyIndex, MainSceneManager.Instance.Player.ATK, currentBossIndex, System.DateTime.Now, isFirstPlay);
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
        saveData.loadData(out weapons, out works, out money, out killCount, out nowEnemyIndex, out atk, out currentBossIndex, out lastconnectTime, out isFirstPlay);
        
        if(MainSceneManager.Instance != null)
        MainSceneManager.Instance.Player.ATK = atk;

        if(isFirst && !isFirstPlay)
        {
            isOpenRewardTap = true;
            isFirst = false;
            long rewardMoney = 0;
            GameObject rewardPanelGameObj = Instantiate(rewardPanelPrefab, GameObject.Find("MainCanvas").transform);
            RewardPanel rewardPanel = rewardPanelGameObj.GetComponent<RewardPanel>();
            rewardPanel.gameObject.SetActive(true);

            for (int i = 0; i < works.Count; i++)
            {
                WorkData current = works[workNames[i]];
                if (current.Isunlocked == false)
                    continue;


                rewardMoney += (int)((System.DateTime.Now - lastconnectTime).TotalSeconds / current.Moneycool) * current.Yield;
            }

            rewardPanel.InitRewardPanel((System.DateTime.Now - lastconnectTime).TotalHours, rewardMoney);
        }

    }

    #endregion
}
