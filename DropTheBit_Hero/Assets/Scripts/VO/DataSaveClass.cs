using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DataSaveClass
{
    public List<string> weaponNameList = new List<string>();
    public List<WeaponData> weaponDatas = new List<WeaponData>();

    public List<string> workNameList = new List<string>();
    public List<WorkData> workDatas = new List<WorkData>();

    public long money;
    public int killCount;
    public int nowEnemyIndex;
    public float atk;
    public ushort currentBossIndex;

    public DataSaveClass() { }
    public DataSaveClass(Dictionary<string, WeaponData> weaponDictionary, Dictionary<string, WorkData> workDictionary, long money, int killCount, int nowEnemyIndex, float atk, ushort currentBossIndex)
    {
        for (int i = 0; i < weaponDictionary.Count; i++)
        {
            this.weaponNameList.Add(weaponDictionary.Keys.ToArray()[i]);
            this.weaponDatas.Add(weaponDictionary.Values.ToArray()[i]);
        }

        for (int i = 0; i < workDictionary.Count; i++)
        {
            this.workNameList.Add(workDictionary.Keys.ToArray()[i]);
            this.workDatas.Add(workDictionary.Values.ToArray()[i]);
        }

        this.money = money;
        this.killCount = killCount;
        this.nowEnemyIndex = nowEnemyIndex;
        this.atk = atk;
        this.currentBossIndex = currentBossIndex;
    }

    public void InitSaveClass(Dictionary<string, WeaponData> weaponDictionary, Dictionary<string, WorkData> workDictionary, long money, int killCount, int nowEnemyIndex, float atk)
    {
        for (int i = 0; i < weaponDictionary.Count; i++)
        {
            weaponDictionary[weaponNameList[i]] = this.weaponDatas[i];
        }

        for (int i = 0; i < workDictionary.Count; i++)
        {
            workDictionary[workNameList[i]] = this.workDatas[i];
        }

        this.money = money;
        this.killCount = killCount;
        this.nowEnemyIndex = nowEnemyIndex;
        this.atk = atk;

    }

    public void loadData(out Dictionary<string, WeaponData> weaponDictionary, out Dictionary<string, WorkData> workDictionary, out long money, out int killCount, out int nowEnemyIndex, out float atk, out ushort currentBossIndex)
    {
        weaponDictionary = new Dictionary<string, WeaponData>();
        for (int i = 0; i < this.weaponNameList.Count; i++)
        {
            weaponDictionary.Add(weaponNameList[i], weaponDatas[i]);
        }

        workDictionary = new Dictionary<string, WorkData>();
        for (int i = 0; i < this.workNameList.Count; i++)
        {
            workDictionary.Add(workNameList[i], workDatas[i]);
        }

        money = this.money;
        killCount = this.killCount;
        nowEnemyIndex = this.nowEnemyIndex;
        atk = this.atk;
        currentBossIndex = this.currentBossIndex;
    }
}