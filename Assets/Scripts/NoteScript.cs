using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect가 뜰 거리")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect 판정에서 몇까지 Good인가?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good 판정에서 몇까지 Miss인가?")] [SerializeField] float whereIsMiss = 0f;
    private AudioSource hitSound;

    public RhythmManager rhythmManager = null;

    private float moveSpeed = 0;
    public void SetRhythmManager(RhythmManager rhythm)
    {
        rhythmManager = rhythm;
        hitSound = rhythm.GetComponents<AudioSource>()[1];
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void OnDisable()
    {
        hitSound.Play();
    }

    private void Update()
    {
        transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0));

        if ( this.transform.position.x + 0.5 <= rhythmManager.noteLine.transform.position.x - (whereIsPerfect + whereIsGood))
        {
            rhythmManager.CheckNote();
        }
    }

    public int isHit(Vector2 linePos) // Perfect = 1, Good = 2, Miss = 3, None = 4
    {
        if (Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect)
        {
            return 1;
        }
        else if (Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect + whereIsGood)
        {
            return 2;
        }
        else if (Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect + whereIsGood + whereIsMiss)
        {
            return 3;
        }
        else if (Vector2.Distance(linePos, this.transform.position) >= whereIsPerfect + whereIsGood + whereIsMiss && transform.position.x >= linePos.x)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }
}
