using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect가 뜰 거리")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect 판정에서 몇까지 Good인가?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good 판정에서 몇까지 Miss인가?")] [SerializeField] float whereIsMiss = 0f;

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
            Debug.Log("판정 범위 밖으로 벗어나 리셋합니다.");
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
