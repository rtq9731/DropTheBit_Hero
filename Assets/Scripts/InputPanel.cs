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
    /// InputPanel�� �ʱ�ȭ �����ݴϴ�. �� �Ŀ� GetBtnOK �ؼ� Ȯ�� ���������� �ൿ�� �������ּ���.
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
