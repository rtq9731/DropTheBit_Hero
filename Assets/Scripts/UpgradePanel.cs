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
    [SerializeField] Button upgradeBtn;
    [SerializeField] GameObject lockPanel;

    int upgradeCost = 0;
    short upgradeCount = 0;
    short index = 0;
    public bool isUnlocked = false;

    public void InitUpgradePanel(string weaponName, short upgradeCount, int upgradeCost, short index, bool isUnlocked)
    {
        this.weaponName.text = weaponName;
        this.upgradeCount = upgradeCount;
        this.upgradeCost = upgradeCost;
        this.index = index;
        this.isUnlocked = isUnlocked;
        Refresh(this.isUnlocked);
    }

    public void Refresh(bool isUnlocked)
    {
        // this.weaponImage.sprite = Resources.Load($"Images/{weaponName.text}") as Sprite;
        if(upgradeCount == 0)
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost}원"; // 업그레이드가 안된 경우 그냥 원가만 표시
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost * upgradeCount}원"; // 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        }


        if(upgradeCount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "최대로 업그레이드 됨!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.UnlockNewWeapon(this.index, this);
        }
        else
        {
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {upgradeCount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }

        OnOffLockPanel(isUnlocked);
    } // 원본함수
    public void Refresh()
    {
        // this.weaponImage.sprite = Resources.Load($"Images/{weaponName.text}") as Sprite;
        if (upgradeCount == 0)
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost}원"; // 업그레이드가 안된 경우 그냥 원가만 표시
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost * upgradeCount}원"; // 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        }


        if (upgradeCount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "최대로 업그레이드 됨!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.UnlockNewWeapon(this.index, this);
        }
        else
        {
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {upgradeCount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }
    } // 오버로딩

    private void OnOffLockPanel(bool isOn)
    {
        lockPanel.SetActive(!isOn);
    }

    private void Upgrade()
    {

        if(upgradeCount == 0)
        {
            if (GameManager.Instance.GetMoney() < upgradeCost) // 돈이 적으면 취소
            {
                return;
            }
            GameManager.Instance.AddMoney(-upgradeCost);
        }
        else
        {
            if (GameManager.Instance.GetMoney() < upgradeCost * upgradeCost) // 돈이 적으면 취소
            {
                return;
            }
            GameManager.Instance.AddMoney(-upgradeCost * upgradeCount);
        }

        ++upgradeCount;

        MainSceneManager.Instance.Player.ATK += index + upgradeCount * 0.5f;
        upgradeBtn.onClick.RemoveAllListeners();
        Debug.Log(upgradeCount);
        Refresh();
    }
}
