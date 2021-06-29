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

    public void SetRhythmManager(RhythmManager rhythm)
    {
        rhythmManager = rhythm;
        hitSound = rhythm.GetComponents<AudioSource>()[1];
    }

    private float moveSpeed = 0;
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0));

        if ( this.transform.position.x <= rhythmManager.noteLine.transform.position.x && Vector2.Distance(transform.position, rhythmManager.noteLine.transform.position) >= whereIsPerfect + whereIsGood + whereIsMiss)
        {
            rhythmManager.CheckNote();
        }
    }

    public int isHit(Vector2 linePos) // Perfect = 1, Good = 2, Miss = 3, None = 4
    {
        if ((linePos - (Vector2)gameObject.transform.position).magnitude <= whereIsPerfect)
        {
            hitSound.Play();
            return 1;
        }
        else if ((linePos - (Vector2)gameObject.transform.position).magnitude <= whereIsPerfect + whereIsGood)
        {
            hitSound.Play();
            return 2;
        }
        else if ((linePos - (Vector2)gameObject.transform.position).magnitude <= whereIsPerfect + whereIsGood + whereIsMiss)
        {
            return 3;
        }
        else if ((linePos - (Vector2)gameObject.transform.position).magnitude >= whereIsPerfect + whereIsGood + whereIsMiss && transform.position.x >= linePos.x)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }
}
