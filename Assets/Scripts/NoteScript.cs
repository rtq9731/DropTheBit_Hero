using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect�� �� �ּ� ������")] [SerializeField] float minPerfectSize = 0f;
    [Header("Perfect�� �� �ִ� ������")] [SerializeField] float maxPerfectSize = 0f;
    [Header("Perfect �������� ����� Good�ΰ�")] [SerializeField] float whereIsGood = 0f;
    [SerializeField] [Header("�ʱ� ũ�� ����")] float noteScale = 2;
     
    void OnEnable()
    {
        this.gameObject.transform.localScale = Vector3.one * noteScale;
    }

    public int isHit() // None = 0 , Perfect = 1, Good = 2, Miss = 3
    {
        if (this.transform.localScale.x > maxPerfectSize + whereIsGood) // �־ ������ �Ȱ� ��
        {
            return 0;
        }
        
        if (this.transform.localScale.x <= maxPerfectSize + whereIsGood && this.transform.localScale.x > maxPerfectSize) // ��¦ ���� Good
        {
            MainSceneManager.Instance.DoHit();
            return 2;
        }
       
        if (this.transform.localScale.x <= maxPerfectSize && this.transform.localScale.x >= minPerfectSize) // Perfect ����
        {
            MainSceneManager.Instance.DoHit();
            return 1;
        }

        if (this.transform.localScale.x < minPerfectSize && this.transform.localScale.x >= minPerfectSize - whereIsGood) // ��¦ �ʾ Good
        {
            MainSceneManager.Instance.DoHit();
            return 2;
        }

        if (this.transform.localScale.x < minPerfectSize - whereIsGood) // �ʹ� �ʾ Miss
        {
            return 3;
        }

        return 3;
    }
}
