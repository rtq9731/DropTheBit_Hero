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

    public WeaponsData data;

    public void InitUpgradePanel(int index)
    {
        data = GameManager.Instance.GetWeaponByIndex(index);
        weaponImage.sprite = Resources.Load<Sprite>(data.Image_Path);
        weaponName.text = data.Name;
        Refresh(data.Isunlocked);
    }

    /// <summary>
    /// 메세지들을 새로고침 해줍니다.
    /// </summary>
    public void Refresh(bool isUnlocked) // 원본함수
    {
        lockPanel.SetActive(!isUnlocked); // isUnlocked와 반대로 움직여야함.

        data.Isunlocked = isUnlocked;
        upgradeBtn.onClick.RemoveAllListeners();

        if(data.Upgradecount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "MAX!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.upgradeCostText.text = $"업그레이드 비용 : 최대로 업그레이드 됨!";
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.GetUpgradePanelByIndex(data.Index).Refresh(true);
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {data.Upgradecost * data.Upgradecount}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {data.Upgradecount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }
    }

    private void Refresh() // 오버로딩 ( 내부용 )
    {
        upgradeBtn.onClick.RemoveAllListeners();


        if (data.Upgradecount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "MAX!"; // 최대로 업그레이드 됐다면 다음으로 넘김
            this.upgradeCostText.text = $"업그레이드 비용 : 최대로 업그레이드 됨!";
            this.currentUpgradeText.text = "현재 업그레이드 단계 : 최대"; // 최대로 업그레이드 됨으로 표시
            MainSceneManager.Instance.upgradeUI.GetUpgradePanelByIndex(data.Index).Refresh(true);
        }
        else
        {
            this.upgradeCostText.text = $"업그레이드 비용 : {data.Upgradecost * data.Upgradecount}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
            this.currentUpgradeText.text = $"현재 업그레이드 단계 : {data.Upgradecount}"; // 업그레이드 단계 표시
            upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
        }
    }

    private void Upgrade()
    {
        {
            if (GameManager.Instance.GetMoney() < data.Upgradecost * data.Upgradecount) // 돈이 적으면 취소
            {
                return;
            }

            GameManager.Instance.AddMoney(-(data.Upgradecost * data.Upgradecount));
        }

        Debug.Log(GameManager.Instance.GetWeaponByIndex(data.Index).Upgradecount);
        ++data.Upgradecount;
        MainSceneManager.Instance.Player.ATK += data.Index + data.Upgradecount * 0.5f;
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }

    public bool GetIsUnlocked()
    {
        if(data.Isunlocked)
        {
            return true;
        }

        return false;
    }
}
