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
            this.upgradeCostText.text = $"���׷��̵� ��� : {upgradeCost}��"; // ���׷��̵尡 �ȵ� ��� �׳� ������ ǥ��
        }
        else
        {
            this.upgradeCostText.text = $"���׷��̵� ��� : {upgradeCost * upgradeCount}��"; // ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        }


        if(upgradeCount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "�ִ�� ���׷��̵� ��!"; // �ִ�� ���׷��̵� �ƴٸ� �������� �ѱ�
            this.currentUpgradeText.text = "���� ���׷��̵� �ܰ� : �ִ�"; // �ִ�� ���׷��̵� ������ ǥ��
            MainSceneManager.Instance.upgradeUI.UnlockNewWeapon(this.index, this);
        }
        else
        {
            this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {upgradeCount}"; // ���׷��̵� �ܰ� ǥ��
            upgradeBtn.onClick.AddListener(() => Upgrade()); // �ִ�� ���׷��̵� ���� �ʾҴٸ� ���׷��̵� ����
        }

        OnOffLockPanel(isUnlocked);
    } // �����Լ�
    public void Refresh()
    {
        // this.weaponImage.sprite = Resources.Load($"Images/{weaponName.text}") as Sprite;
        if (upgradeCount == 0)
        {
            this.upgradeCostText.text = $"���׷��̵� ��� : {upgradeCost}��"; // ���׷��̵尡 �ȵ� ��� �׳� ������ ǥ��
        }
        else
        {
            this.upgradeCostText.text = $"���׷��̵� ��� : {upgradeCost * upgradeCount}��"; // ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        }


        if (upgradeCount >= 5)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "�ִ�� ���׷��̵� ��!"; // �ִ�� ���׷��̵� �ƴٸ� �������� �ѱ�
            this.currentUpgradeText.text = "���� ���׷��̵� �ܰ� : �ִ�"; // �ִ�� ���׷��̵� ������ ǥ��
            MainSceneManager.Instance.upgradeUI.UnlockNewWeapon(this.index, this);
        }
        else
        {
            this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {upgradeCount}"; // ���׷��̵� �ܰ� ǥ��
            upgradeBtn.onClick.AddListener(() => Upgrade()); // �ִ�� ���׷��̵� ���� �ʾҴٸ� ���׷��̵� ����
        }
    } // �����ε�

    private void OnOffLockPanel(bool isOn)
    {
        lockPanel.SetActive(!isOn);
    }

    private void Upgrade()
    {

        if(upgradeCount == 0)
        {
            if (GameManager.Instance.GetMoney() < upgradeCost) // ���� ������ ���
            {
                return;
            }
            GameManager.Instance.AddMoney(-upgradeCost);
        }
        else
        {
            if (GameManager.Instance.GetMoney() < upgradeCost * upgradeCost) // ���� ������ ���
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
