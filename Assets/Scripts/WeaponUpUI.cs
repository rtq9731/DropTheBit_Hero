using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpUI : MonoBehaviour
{
    [SerializeField] GameObject upgradePanelPrefab;
    [SerializeField] GameObject lockObject;

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
}
