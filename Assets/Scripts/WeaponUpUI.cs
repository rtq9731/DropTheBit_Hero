using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpUI : MonoBehaviour
{
    [SerializeField] GameObject upgradePanelPrefab;
    [SerializeField] List<UpgradePanel> upgradePanels;
    [SerializeField] GameObject lockObject;

    Dictionary<string, WeaponsData> weapons;

    private void Start()
    {
        MakeUpgradePanels();
    }

    public void UnlockNewWeapon(short index, UpgradePanel upgradePanel)
    {
        WeaponsData temp = GameManager.Instance.GetWeponDictionary()[GameManager.Instance.weaponNames[index]];
        upgradePanel.isUnlocked = true;
        upgradePanels[index].Refresh(upgradePanel.isUnlocked);
    }

    public void MakeUpgradePanels()
    {
        weapons = GameManager.Instance.GetWeponDictionary();
        foreach (var item in weapons)
        {
            GameObject temp;
            temp = Instantiate(upgradePanelPrefab, this.transform);
            temp.name = item.Value.Name;
            UpgradePanel tempScript = temp.GetComponent<UpgradePanel>();
            tempScript.InitUpgradePanel(item.Value.Name, item.Value.Upgradecount, item.Value.Upgradecost, item.Value.Index, item.Value.Isunlocked);
            upgradePanels.Add(tempScript);
        }
    }
}
