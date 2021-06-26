using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPanel : MonoBehaviour
{
    [SerializeField] Image workImage;
    [SerializeField] Text workName;
    [SerializeField] Text upgradeCostText;
    [SerializeField] Text currentUpgradeText;
    [SerializeField] Button upgradeBtn;
    [SerializeField] GameObject lockPanel;

    WorkData data = null;

    public void InitWorkPanel(int index)
    {
        data = GameManager.Instance.GetWorkDataByindex(index);
        workImage.sprite = Resources.Load<Sprite>(data.Image_Path);
        workName.text = data.Name;
    }

    /// <summary>
    /// �޼������� ���ΰ�ħ ���ݴϴ�.
    /// </summary>
    public void Refresh(bool isUnlocked) // �����Լ�
    {
        if(data.Isunlocked) // �̹� �رݵǾ��ٸ� 
        {
            return;
        }

        lockPanel.SetActive(!isUnlocked); // isUnlocked�� �ݴ�� ����������.

        data.Isunlocked = isUnlocked;
        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"���׷��̵� ��� : {Mathf.RoundToInt(data.Upgradecost * (0.5f * data.Upgradecount))}��"; // ù ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {data.Upgradecount}"; // ���׷��̵� �ܰ� ǥ��
        upgradeBtn.onClick.AddListener(() => Upgrade()); // �ִ�� ���׷��̵� ���� �ʾҴٸ� ���׷��̵� ����
    }
    public void Refresh() // �����Լ�
    {
        if(data.Upgradecount >= data.Leastup) // �ּ� ���׷��̵� �̻� ���۵Ǹ�
        {
            MainSceneManager.Instance.workUI.GetWorkPanelByIndex(data.Index).Refresh(true);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"���׷��̵� ��� : {Mathf.RoundToInt(data.Upgradecost * ( 0.5f * data.Upgradecount ))}��"; // ù ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {data.Upgradecount}"; // ���׷��̵� �ܰ� ǥ��
        upgradeBtn.onClick.AddListener(() => Upgrade()); // �ִ�� ���׷��̵� ���� �ʾҴٸ� ���׷��̵� ����
    }

    private void Upgrade()
    {
        if (GameManager.Instance.GetMoney() < data.Upgradecost * data.Upgradecount) // ���� ������ ���
        {
            return;
        }

        GameManager.Instance.AddMoney(-Mathf.RoundToInt(data.Upgradecost * (0.5f * data.Upgradecount)));

        ++data.Upgradecount;
        data.Yield += data.Yield * 0.1f;
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }

    public bool GetIsUnlocked()
    {
        return data.Isunlocked;
    }

}
