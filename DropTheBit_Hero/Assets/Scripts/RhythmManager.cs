using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RhythmManager : MonoBehaviour
{
    class HitEffectColors
    {
        public Color perfectColor = new Color(1, 0.8874891f, 0, 1);
        public Color goodColor = new Color(0.6792453f, 0.9150943f, 0.06030021f, 1);
        public Color missColor = new Color(0.6792453f, 0, 0.06030021f, 1);
    }

    [Header("����� ����")]
    [SerializeField] AudioSource audioSource;

    [Header("��Ʈ ����")]
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject longNotePrefab;
    [SerializeField] Transform noteMakeTr;
    [SerializeField] public GameObject noteLine;
    [SerializeField] float noteEndXOffset = 0f;

    private Queue<GameObject> noteObjPool = new Queue<GameObject>();
    private Queue<NoteScript> noteCheckPool = new Queue<NoteScript>();
    private Queue<GameObject> textEffectPool = new Queue<GameObject>();

    private int noteMakeIndex = 0;

    private int currentTimingPointIndex = 0;

    TimingPoint currentTimingPoint = new TimingPoint();

    private float noteTimer = 0;
    private float noteLineDistance = 0f;
    private bool isTimingPointPlay = false;

    [Header("�޺� ����Ʈ ����")]
    [SerializeField] Transform textEffectPoolObj;
    [SerializeField] Transform hitEffectPoolObj;
    [SerializeField] GameObject textEffectPrefab;
    [SerializeField] GameObject hitEffectPrefab;

    private HitEffectColors textEffectColors = new HitEffectColors();

    private bool isPlayingNote = false;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Delete))
            StartCoroutine(FinishRhythm());
#endif

        if(GameManager.Instance.isFinishParshing > 0)
        {
            GameManager.Instance.isFinishParshing--;
            if(isPlayingNote == false )
                StartStopSong();
        }

        if(isTimingPointPlay)
        {
            if (GameManager.Instance.GetCurrentBeatmap().TimingPoints[currentTimingPointIndex].Offset < noteTimer)
            {
                currentTimingPoint = GameManager.Instance.GetCurrentBeatmap().TimingPoints[currentTimingPointIndex];
                ++currentTimingPointIndex;
                if (currentTimingPointIndex == GameManager.Instance.GetCurrentBeatmap().TimingPoints.Count)
                {
                    isTimingPointPlay = false;
                }
            }
        }


        if (isPlayingNote)
        {
            noteTimer += Time.deltaTime * 1000;

            if(GameManager.Instance.GetCurrentBeatmap().HitObjects.Count <= noteMakeIndex)
            {
                StartCoroutine(FinishRhythm());
                return;
            }

            BossSceneManager.Instance.progressBar.maxValue = GameManager.Instance.GetCurrentBeatmap().HitObjects.Count;
            float noteTiming = GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex].Time;
            if (isPlayingNote && noteTimer >= noteTiming - 1000 - currentTimingPoint.MsPerBeat) // ��Ʈ Ÿ������ ���� 1�ʰ� �ɸ����� �����س���. �׷��� 1�� + ������ ���� ����.
            {
                Debug.Log(GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex].GetType().Name);
                if ((GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex].GetType().Name == "HitSlider"))// �ճ�Ʈ�� ���鵵�� �ؾ���
                {
                    ++noteMakeIndex;
                    CrateNote();
                    //noteMakeIndex++;
                    //CreateLongNote((HitSlider)GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex]);
                }
                else // �Ϲ� ��Ʈ
                {
                    ++noteMakeIndex;
                    CrateNote();
                }
            }

        }

        if(Input.GetMouseButtonDown(0))
        {
            CheckNote();
        }
    }

    public void StartStopSong()
    {
        if (!isPlayingNote)
            audioSource.clip = GameManager.Instance.GetMusic();

        isPlayingNote = !isPlayingNote;
        isTimingPointPlay = !isTimingPointPlay;
        noteLineDistance = Vector2.Distance(noteMakeTr.position, noteLine.transform.position);

        if (isPlayingNote)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private IEnumerator FinishRhythm()
    {
        yield return new WaitForSeconds(2f); // ������ ��Ʈ�� ������������ ���.
        StartStopSong();
        isPlayingNote = false;
        BossSceneManager.Instance.FinishFight();
        yield return null;
    }

    void CrateNote()
    {
        int randNum = Random.Range(1, 3);
        switch (randNum)
        {
            case 1:
                BossSceneManager.Instance.bossAnimator.SetTrigger(BossSceneManager.Instance.bossAttack1Hash);
                break;
            case 2:
                BossSceneManager.Instance.bossAnimator.SetTrigger(BossSceneManager.Instance.bossAttack2Hash);
                break;
            default:
                break;
        }

        GameObject currentNote;
        if(noteObjPool.Count > 0)
        {
            if (noteObjPool.Peek().activeSelf)
            {
                currentNote = Instantiate(notePrefab, transform);
            }
            else
            {
                currentNote = noteObjPool.Dequeue();
            }
        }
        else
        {
            currentNote = Instantiate(notePrefab, transform);
        }

        Transform noteTr = currentNote.transform;
        noteTr.position = noteMakeTr.position;
        NoteScript nc = noteTr.GetComponent<NoteScript>();
        noteCheckPool.Enqueue(nc);
        nc.SetRhythmManager(this);
        nc.SetSpeed(noteLineDistance);
        currentNote.SetActive(true);
        noteObjPool.Enqueue(currentNote);

    }

    //void CreateLongNote(HitSlider slider)
    //{

    //    if (notesforPooling.Count < poolingMax)
    //    {
    //        longNoteList.Add(Instantiate(longNotePrefab, transform));
    //    }
    //    else
    //    {
    //        longNoteList[longNoteMakeIndex].gameObject.SetActive(true);
    //    }

    //    longNoteList[longNoteMakeIndex].gameObject.transform.position = noteMakeTr.position;
    //    LongNoteScript longNote = longNoteList[longNoteMakeIndex].GetComponent<LongNoteScript>();
    //    float longNoteLength = GetSliderLengthInMs(slider);
    //    longNote.InitLongNote(noteLine.transform.position, noteLineDistance, longNoteLength, noteMakeTr.position); // ��Ʈ �ʱ�ȭ

    //    longNoteMakeIndex++;
    //    if (longNoteMakeIndex == longNotePoolingMax)
    //    {
    //        longNoteMakeIndex = 0; // longNoteMakeIndex�� �ִ�ġ��� �ٽ� �ʱ�ȭ
    //    }

    //}

    //float GetSliderLengthInMs(HitSlider slider)
    //{
    //    var pixelsPerBeat = GameManager.Instance.GetCurrentBeatmap().SliderMultiplier * currentTimingPoint.MsPerBeat;
    //    var sliderLengthInBeats = (slider.PixelLength * slider.Repeat) / pixelsPerBeat;
    //    return pixelsPerBeat * sliderLengthInBeats;
    //}

     void CrateEffect(int n) // Perfect = 1, Good = 2, Miss = 3
    {
        GameObject current;
        if(textEffectPool.Count > 0)
        {
            if (textEffectPool.Peek().activeSelf)
            {
                current = Instantiate(textEffectPrefab, textEffectPoolObj);
            }
            else
            {
                current = textEffectPool.Dequeue();
            }
        }
        else
        {
            current = Instantiate(textEffectPrefab, textEffectPoolObj);
        }

        current.SetActive(true);
        NoteHitEffect nowEffect = current.GetComponent<NoteHitEffect>();
        switch (n)
        {
            case 1:
                {
                    nowEffect.text.color = textEffectColors.perfectColor;
                    GameManager.Instance.Combo += 2;
                    nowEffect.text.text = $"PERFECT X {GameManager.Instance.Combo}";

                    BossSceneManager.Instance.progressBar.value += 2;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);

                    break;
                }
            case 2:
                {
                    nowEffect.text.color = textEffectColors.goodColor;
                    ++GameManager.Instance.Combo;
                    nowEffect.text.text = $"GOOD X {GameManager.Instance.Combo}";

                    BossSceneManager.Instance.progressBar.value++;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);

                    break;
                }
            case 3:
                {
                    nowEffect.text.color = textEffectColors.missColor;
                    GameManager.Instance.Combo = 0;
                    nowEffect.text.text = "MISS";
                    
                    BossSceneManager.Instance.progressBar.value--;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerHitHash);
                    

                    break;
                }
        }

        nowEffect.transform.position = textEffectPoolObj.position;

        {
            Sequence seq = DOTween.Sequence();
            seq.OnStart(() => {
                nowEffect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            });
            seq.Append(nowEffect.GetComponent<RectTransform>().DOAnchorPosY(50, 1).SetEase(Ease.InBack));
            seq.Join(nowEffect.text.DOFade(0, 1));
            seq.OnComplete(() => nowEffect.gameObject.SetActive(false));
        }

        textEffectPool.Enqueue(current);
    }

    void DeleteNote(NoteScript note)
    {
        note.gameObject.SetActive(false);
        DOTween.Complete(note.gameObject);
    }

    public void CheckNote()
    {
        NoteScript currentNote = noteCheckPool.Peek();
        if (currentNote.isHit(noteLine.transform.position) % 4 > 0)
        {
            CrateEffect(currentNote.isHit(noteLine.transform.position) % 4);
            DeleteNote(noteCheckPool.Dequeue());
        }
        else
        {
            return;
        }
        //for (int i = 0; i < notesforPooling.Count; i++)
        //{
        //    var item = notesforPooling[i];
        //    if (item.activeSelf)
        //    {
        //        if ((item.GetComponent<NoteScript>().isHit(noteLine.transform.position) % 4) > 0) // None�� �ƴ� ���
        //        {
        //            //Camera.main.transform.DOShakePosition(shakeTime, shakePower / (hit / 4));
        //            CrateEffect(item.GetComponent<NoteScript>().isHit(noteLine.transform.position) % 4);
        //            DeleteNote(item.gameObject);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}


    }

}
