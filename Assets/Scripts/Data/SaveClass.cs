using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveClass
{
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();
    private float atk = 0;
    private int killCount = 0;
    private decimal money = 0;


    public void InitSaveClass(Dictionary<string, WeaponsData> weapons, float atk, int killCount, decimal money)
    {
        this.weapons = weapons;
    }
}
