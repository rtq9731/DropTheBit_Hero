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
        Debug.Log("HI");
        rhythmManager = rhythm;
    }

    private void Update()
    {
        if(rhythmManager.transform.position.x + transform.position.x <= rhythmManager.transform.position.x - whereIsMiss)
            rhythmManager.CheckNote();
    }

    public int isHit(Vector2 linePos) // Perfect = 1, Good = 2, Miss = 3, None = 4
    {
        Debug.Log(Vector2.Distance(linePos, this.transform.position));
        if(Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect)
        {
            rhythmManager.CrateEffect("PERFECT");
            return 1;
        }
        else if (Vector2.Distance(linePos, this.transform.position) <= whereIsGood)
        {
            rhythmManager.CrateEffect("GOOD");
            return 2;
        }
        else if(Vector2.Distance(linePos, this.transform.position) <= whereIsMiss)
        {
            rhythmManager.CrateEffect("MISS");
            return 3;
        }
        else if (Vector2.Distance(linePos, this.transform.position) >= whereIsMiss)
        {
            return 4;
        }

        return 3;
    }
}
