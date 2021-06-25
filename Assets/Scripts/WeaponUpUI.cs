using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpUI : MonoBehaviour
{
    [SerializeField] GameObject upgradePanelPrefab;
    [SerializeField] GameObject lockObject;

    List<UpgradePanel> panels = new List<UpgradePanel>();

    private void Start()
    {
        MakeUpgradePanels();
    }

    void MakeUpgradePanels()
    {
        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            GameObject temp = Instantiate(upgradePanelPrefab, transform);
            temp.GetComponent<UpgradePanel>().InitUpgradePanel(i);
        }
    }

    public short GetCurrentWeapon()
    {
        short temp = 0;
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i].GetIsUnlocked())
                temp++;
        }

        Debug.Log(temp);
        return temp;
    }
}
