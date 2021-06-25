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
    short upgradeCount = 1;
    short weaponIndex = 0;
    public bool isUnlocked = false;

    public void InitUpgradePanel(string imageDataPath, string weaponName, short upgradeCount, int upgradeCost, short weaponIndex, bool isUnlocked)
    {
        weaponImage.sprite = Resources.Load(imageDataPath, typeof(Sprite)) as Sprite;
        Debug.Log(weaponImage.sprite);
        this.weaponName.text = weaponName;
        this.upgradeCount = upgradeCount;
        this.upgradeCost = upgradeCost;
        this.weaponIndex = weaponIndex;
        this.isUnlocked = isUnlocked;
        Refresh(this.isUnlocked);
    }

    /// <summary>
    /// 메세지들을 새로고침 해줍니다.
    /// </summary>
    public void Refresh(bool isUnlocked) // 원본함수
    {
        upgradeBtn.onClick.RemoveAllListeners();

        if (upgradeCount < 5)
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost * upgradeCount}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : 최대로 업그레이드 됨!";
        }


        if(upgradeCount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "MAX!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.UnlockNewWeapon(this.weaponIndex, this);
        }
        else
        {
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {upgradeCount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }

        OnOffLockPanel(isUnlocked);
    }


    /// <summary>
    /// 메세지들을 새로고침 해줍니다.
    /// </summary>
    private void Refresh() // 오버로딩 ( 내부용 )
    {
        upgradeBtn.onClick.RemoveAllListeners();
        
        this.upgradeCostText.text = $"업그레이드 비용 : {upgradeCost * upgradeCount}원"; // 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시


        if (upgradeCount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "MAX!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.UnlockNewWeapon(this.weaponIndex, this);
        }
        else
        {
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {upgradeCount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }
    }

    private void OnOffLockPanel(bool isOn)
    {
        lockPanel.SetActive(!isOn);
    }

    private void Upgrade()
    {
        {
            if (GameManager.Instance.GetMoney() < upgradeCount * upgradeCost) // 돈이 적으면 취소
            {
                return;
            }

            GameManager.Instance.AddMoney(-(upgradeCost * upgradeCount));
        }
        ++upgradeCount;
        MainSceneManager.Instance.Player.ATK += weaponIndex + upgradeCount * 0.5f;
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }
}
