using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private float money;
    private float stage;

    public void AddMoney(float addNum)
    {
        money += addNum;
    }
}
