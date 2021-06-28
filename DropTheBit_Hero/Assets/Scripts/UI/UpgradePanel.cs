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

    int index = 0;

    public void InitUpgradePanel(int index)
    {
        this.index = index;
        weaponImage.sprite = Resources.Load<Sprite>(GameManager.Instance.GetWeaponByIndex(index).Image_Path);
        weaponName.text = GameManager.Instance.GetWeaponByIndex(index).Name;
    }

    /// <summary>
    /// 메세지들을 새로고침 해줍니다.
    /// </summary>
    public void Refresh(bool isUnlocked) // 원본함수
    {
        lockPanel.SetActive(!isUnlocked); // isUnlocked와 반대로 움직여야함.

        GameManager.Instance.GetWeaponByIndex(index).Isunlocked = isUnlocked;
        upgradeBtn.onClick.RemoveAllListeners();

        if(GameManager.Instance.GetWeaponByIndex(index).Upgradecount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "MAX!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.upgradeCostText.text = $"업그레이드 비용 : 최대";
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.GetUpgradePanelByIndex(GameManager.Instance.GetWeaponByIndex(index).Index).Refresh(true);
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {GameManager.Instance.GetWeaponByIndex(index).Upgradecost * GameManager.Instance.GetWeaponByIndex(index).Upgradecount}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {GameManager.Instance.GetWeaponByIndex(index).Upgradecount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }
    }

    private void Refresh() // 오버로딩 ( 내부용 )
    {
        upgradeBtn.onClick.RemoveAllListeners();

        if (GameManager.Instance.GetWeaponByIndex(index).Upgradecount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "MAX!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.upgradeCostText.text = $"업그레이드 비용 : 최대";
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.GetUpgradePanelByIndex(GameManager.Instance.GetWeaponByIndex(index).Index).Refresh(true);
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {GameManager.Instance.GetWeaponByIndex(index).Upgradecost * GameManager.Instance.GetWeaponByIndex(index).Upgradecount}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {GameManager.Instance.GetWeaponByIndex(index).Upgradecount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }
    }

    private void Upgrade()
    {
        {
            if (GameManager.Instance.GetMoney() < GameManager.Instance.GetWeaponByIndex(index).Upgradecost * GameManager.Instance.GetWeaponByIndex(index).Upgradecount) // 돈이 적으면 취소
            {
                return;
            }

            GameManager.Instance.AddMoney(-(GameManager.Instance.GetWeaponByIndex(index).Upgradecost * GameManager.Instance.GetWeaponByIndex(index).Upgradecount));
        }

        ++GameManager.Instance.GetWeaponByIndex(index).Upgradecount;
        MainSceneManager.Instance.Player.ATK += GameManager.Instance.GetWeaponByIndex(index).Index + GameManager.Instance.GetWeaponByIndex(index).Upgradecount * 0.5f;
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }

    public bool GetIsUnlocked()
    {
        if(GameManager.Instance.GetWeaponByIndex(index).Isunlocked)
        {
            return true;
        }

        return false;
    }
}
