using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

    private int money;
    private float stage;
    private int killCount;
    private int weaponChangeCount;
    private Dictionary<string, int> waponUpgradeCountbyName = new Dictionary<string, int>();

    public int KillCount { get { return killCount; } set { killCount = value; MainSceneManager.Instance.topUI.UpdateCurrentKillCount(); } }

    public void AddMoney(int addNum)
    {
        money += addNum;
        MainSceneManager.Instance.topUI.UpdateCurrentCoin();
    }
    public int GetMoney()
    {
        return money;
    }
}
