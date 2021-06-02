using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;
    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] MonsterData excelSheet;

    private Dictionary<string, MonsterDataData> monsterDatafromExcel = new Dictionary<string, MonsterDataData>();
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
            Debug.Log(monsterDatafromExcel[excelSheet.dataArray[i].Name].Name);
        }
    }

    public MonsterDataData GetEnemyDataFromName(string name)
    {
        if(monsterDatafromExcel[name] != null)
        {
            return monsterDatafromExcel[name];
        }
        else
        {
            return null;
#if UNITY_EDITOR
            Debug.Log($"can't find the Monster by {name}");
#endif
        }
    }
}
