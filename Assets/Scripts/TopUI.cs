using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : MonoBehaviour
{
    [SerializeField] Text coinText;
    [SerializeField] Text killText;
    
    private void Awake()
    {
        UpdateCurrentCoin();
    }

    public void UpdateCurrentCoin()
    {
        coinText.text = $"���� �� : {GameManager.Instance.GetMoney()}";
    }

    public void UpdateCurrentKillCount()
    {
        killText.text = $"��� �� : {GameManager.Instance.KillCount}";
    }
}
