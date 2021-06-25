using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveClass
{
    List<string> weaponDictionaryNames;
    List<WeaponsData> weaponDictinaryDatas;
    private float atk = 0;
    private int killCount = 0;
    private decimal money = 0;
    
    public SaveClass(Dictionary<string, WeaponsData> weapons, float atk, int killCount, decimal money)
    {
        foreach (var item in weapons)
        {
            weaponDictionaryNames.Add(item.Key);
            weaponDictinaryDatas.Add(item.Value);
        }

        this.atk = atk;
        this.killCount = killCount;
        this.money = money;
    }
}
