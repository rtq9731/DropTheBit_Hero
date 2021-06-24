using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveClass
{
    private Dictionary<string, WeaponsData> weapons = new Dictionary<string, WeaponsData>();

    public void InitSaveClass(Dictionary<string, WeaponsData> weapons)
    {
        this.weapons = weapons;
    }
}
