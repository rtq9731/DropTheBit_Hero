using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect�� �� �Ÿ�")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect �������� ����� Good�ΰ�?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good �������� ����� Miss�ΰ�?")] [SerializeField] float whereIsMiss = 0f;

    public RhythmManager rhythmManager = null;

    public void SetRhythmManager(RhythmManager rhythm)
    {
        rhythmManager = rhythm;
        Debug.Log(rhythmManager.noteLine.transform.position.x - (whereIsPerfect + whereIsGood));
    }

    private void Update()
    {
        if ( this.transform.position.x + 0.5 <= rhythmManager.noteLine.transform.position.x - (whereIsPerfect + whereIsGood))
        {
            Debug.Log("���� ���� ������ ��� �����մϴ�.");
            rhythmManager.CheckNote();
        }
    }

    public int isHit(Vector2 linePos) // Perfect = 1, Good = 2, Miss = 3, None = 4
    {
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
        else if (Vector2.Distance(linePos, this.transform.position) >= whereIsMiss && transform.position.x >= rhythmManager.transform.position.x)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }
}
