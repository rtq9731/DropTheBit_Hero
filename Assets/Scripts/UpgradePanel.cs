using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] Image weaponImage;
    [SerializeField] Text weaponName;
    [SerializeField] Text upgradeCostText;
    [SerializeField] Text currentUpgradeText;

    int upgradeCost = 0;
    short upgradeCount = 0;

    UpgradePanel(string weaponName, short upgradeCount, int upgradeCost)
    {
        this.weaponName.text = weaponName;
        this.upgradeCount = upgradeCount;
        this.upgradeCost = upgradeCost;
    }

    public void Refresh()
    {
        this.weaponImage.sprite = Resources.Load($"Images/{weaponName.text}") as Sprite;
        this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost * upgradeCount}원";
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {upgradeCount}";
    }
}
