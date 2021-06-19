using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InputPanel : MonoBehaviour
{
    [SerializeField] Button btnOK;
    [SerializeField] Button btnCancel;
    [SerializeField] GameObject realInputPanel;
    [SerializeField] Text message;

    private void OnEnable()
    {
        realInputPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f);
    }

    /// <summary>
    /// InputPanel을 초기화 시켜줍니다. 꼭 후에 GetBtnOK 해서 확인 눌렀을때의 행동을 정의해주세요.
    /// </summary>
    public void InitInputPanel(string message)
    {
        this.message.text = message;
        btnCancel.onClick.AddListener(() => gameObject.GetComponent<RectTransform>().DOAnchorPosY(2000, 0.5f).OnComplete(() => gameObject.SetActive(false)));
    }

    public Button GetBtnOk()
    {
        return btnOK;
    }
}
