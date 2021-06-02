using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    [Header("자체 접근용")]
    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] Monster excelSheet;

    [Header("다른 오브젝트 접근용")]
    [SerializeField] public TopUI topUI;

    private Dictionary<string, MonsterData> monsterDatafromExcel = new Dictionary<string, MonsterData>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;
        InputCommonEnemyData();
    }
    void OnDestroy()
    {
        Instance = null;
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
        for (int i = 0; i < excelSheet.dataArray.Length; i++)
        {
            monsterDatafromExcel.Add(excelSheet.dataArray[i].Name, excelSheet.dataArray[i]);
        }
    }

    public void GetEnemyDataFromName(string name, out float hp, out int cost)
    {
        Debug.Log(name);
        MonsterData tempData = monsterDatafromExcel[name];
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
