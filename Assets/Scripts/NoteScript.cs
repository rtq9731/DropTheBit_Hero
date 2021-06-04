using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect�� �� �ּ� ������")] [SerializeField] float minPerfectSize = 0f;
    [Header("Perfect�� �� �ִ� ������")] [SerializeField] float maxPerfectSize = 0f;
    [Header("Perfect �������� ����� Good�ΰ�")] [SerializeField] float whereIsGood = 0f;

    public int isHit() // None = 0 , Perfect = 1, Good = 2, Miss = 3
    {
        if (this.transform.localScale.x > maxPerfectSize + whereIsGood) // �־ ������ �Ȱ� ��
        {
            Debug.Log("None!");
            return 0;
        }
        
        if (this.transform.localScale.x <= maxPerfectSize + whereIsGood && this.transform.localScale.x > maxPerfectSize) // ��¦ ���� Good
        {
            Debug.Log("GOOD!");
            return 2;
        }
       
        if (this.transform.localScale.x <= maxPerfectSize && this.transform.localScale.x >= minPerfectSize) // Perfect ����
        {
            Debug.Log("PERFECT!");
            return 1;
        }

        if (this.transform.localScale.x < minPerfectSize && this.transform.localScale.x >= minPerfectSize - whereIsGood) // ��¦ �ʾ Good
        {
            Debug.Log("GOOD!");
            return 2;
        }

        if (this.transform.localScale.x < minPerfectSize - whereIsGood) // �ʹ� �ʾ Miss
        {
            Debug.Log("MISS!");
            return 3;
        }

        return 3;
    }
}
