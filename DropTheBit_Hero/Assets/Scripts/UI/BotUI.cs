using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BotUI : MonoBehaviour
{
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] Toggle weaponToggle;
    [SerializeField] Toggle workToggle;
    [SerializeField] Toggle callFriends;

    List<GameObject> scrollViews = new List<GameObject>();

    private void Start()
    {
        InitToggles();
        weaponToggle.isOn = true;
    }

    private void InitToggles()
    {
        scrollViews.Add(MainSceneManager.Instance.upgradeUI.GetComponentInChildren<ScrollRect>().transform.gameObject);
        scrollViews.Add(MainSceneManager.Instance.workUI.GetComponentInChildren<ScrollRect>().transform.gameObject);
        scrollViews.ForEach((x) => { x.SetActive(false); });
        weaponToggle.onValueChanged.AddListener((x) => UpdateUI(x, weaponToggle, scrollViews[0]));
        workToggle.onValueChanged.AddListener((x) => UpdateUI(x, workToggle, scrollViews[1]));
        //callFriends.onValueChanged.AddListener((x) => UpdateUI(x, callFriends, scrollViews[2]));
    }

    private void UpdateUI(bool isOn, Toggle toggle, GameObject pannel)
    {
        Debug.Log(isOn);
        if(isOn)
        {
            toggle.GetComponent<RectTransform>().DOAnchorPosY(toggle.GetComponent<RectTransform>().anchoredPosition.y - 30, 0.5f);
            pannel.SetActive(isOn);
        }
        else
        {
            toggle.GetComponent<RectTransform>().DOAnchorPosY(toggle.GetComponent<RectTransform>().anchoredPosition.y + 30, 0.5f);
            pannel.SetActive(isOn);
        }
    }


}
