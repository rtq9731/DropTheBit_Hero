using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect�� �� �Ÿ�")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect �������� ����� Good�ΰ�?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good �������� ����� Miss�ΰ�?")] [SerializeField] float whereIsMiss = 0f;

    private void Update()
    {
        
    }

    public int isHit(Vector2 linePos) // None = 0 , Perfect = 1, Good = 2, Miss = 3
    {
        Debug.Log(Vector2.Distance(linePos, this.transform.position));
        if(Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect)
        {
            return 1;
        }
        else if (Vector2.Distance(linePos, this.transform.position) <= whereIsGood)
        {
            return 2;
        }
        else if(Vector2.Distance(linePos, this.transform.position) <= whereIsMiss)
        {
            return 3;
        }
        else if (Vector2.Distance(linePos, this.transform.position) >= whereIsMiss)
        {
            return 4;
        }

        return 3;
    }
}
