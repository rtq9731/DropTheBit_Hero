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

    [Header("오디오 관련")]
    [SerializeField] AudioSource audioSource;

    [Header("노트 관련")]
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject longNotePrefab;
    [SerializeField] Transform noteMakeTr;
    [SerializeField] public GameObject noteLine;
    [SerializeField] int poolingMax = 5;
    [SerializeField] int longNotePoolingMax = 5;
    [SerializeField] float noteEndXOffset = 0f;

   public string parsingSongName = "";

    private List<GameObject> notesforPooling = new List<GameObject>();
    private List<GameObject> longNoteList = new List<GameObject>();

    private int indexforNotePooling = 0;
    private int noteMakeIndex = 0;
    private int longNoteMakeIndex = 0;
    private int noteCheckIndex = 0;

    private int currentTimingPointIndex = 0;

    TimingPoint currentTimingPoint = new TimingPoint();

    private float noteTimer = 0;
    private float noteLineDistance = 0f;
    private bool isTimingPointPlay = false;

    [Header("콤보 이펙트 관련")]
    [SerializeField] Transform hitEffectTransform;
    [SerializeField] GameObject hitEffectPrefab;

    private List<GameObject> effects = new List<GameObject>();
    private HitEffectColors hitEffectColors = new HitEffectColors();
    private int indexforEffectPooling = 0;

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
            if (GameManager.Instance.parsingManager.BeatmapData[parsingSongName].TimingPoints[currentTimingPointIndex].Offset < noteTimer)
            {
                currentTimingPoint = GameManager.Instance.parsingManager.BeatmapData[parsingSongName].TimingPoints[currentTimingPointIndex];
                ++currentTimingPointIndex;
                if (currentTimingPointIndex == GameManager.Instance.parsingManager.BeatmapData[parsingSongName].TimingPoints.Count)
                {
                    isTimingPointPlay = false;
                }
            }
        }


        if (isPlayingNote)
        {
            noteTimer += Time.deltaTime * 1000;

            if(GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects.Count <= noteMakeIndex)
            {
                StartCoroutine(FinishRhythm());
                return;
            }

            BossSceneManager.Instance.progressBar.maxValue = GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects.Count;
            float noteTiming = GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects[noteMakeIndex].Time;
            if (isPlayingNote && noteTimer >= noteTiming - currentTimingPoint.MsPerBeat) // 노트 타격지점 까지 1초가 걸리도록 설계해놓음. 그래서 오프셋 빼줄 것임.
            {
                Debug.Log(GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects[noteMakeIndex].GetType().Name);
                if ((GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects[noteMakeIndex].GetType().Name == "HitSlider"))// 롱노트를 만들도록 해야함;
                {
                    ++noteMakeIndex;
                    CrateNote();
                    //noteMakeIndex++;
                    //CreateLongNote((HitSlider)GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects[noteMakeIndex]);
                }
                else // 일반 노트
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
        yield return new WaitForSeconds(1.3f); // 마지막 노트가 지나갈때까지 대기.
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

        if (notesforPooling.Count < poolingMax)
        {
            notesforPooling.Add(Instantiate(notePrefab, transform));
        }
        else
        {
            notesforPooling[indexforNotePooling].gameObject.SetActive(true);
        }

        Transform noteTr = notesforPooling[indexforNotePooling].transform;
        noteTr.position = noteMakeTr.position;
        NoteScript nc = noteTr.GetComponent<NoteScript>();
        nc.SetRhythmManager(this);
        nc.SetSpeed(noteLineDistance);

        indexforNotePooling++;
        if (indexforNotePooling == poolingMax)
        {
            indexforNotePooling = 0; // indexforNotePooling가 최대치라면 다시 초기화
        }

    }

    void CreateLongNote(HitSlider slider)
    {

        if (notesforPooling.Count < poolingMax)
        {
            longNoteList.Add(Instantiate(longNotePrefab, transform));
        }
        else
        {
            longNoteList[longNoteMakeIndex].gameObject.SetActive(true);
        }

        longNoteList[longNoteMakeIndex].gameObject.transform.position = noteMakeTr.position;
        LongNoteScript longNote = longNoteList[longNoteMakeIndex].GetComponent<LongNoteScript>();
        float longNoteLength = GetSliderLengthInMs(slider);
        longNote.InitLongNote(noteLine.transform.position, noteLineDistance, longNoteLength, noteMakeTr.position); // 노트 초기화

        longNoteMakeIndex++;
        if (longNoteMakeIndex == longNotePoolingMax)
        {
            longNoteMakeIndex = 0; // longNoteMakeIndex가 최대치라면 다시 초기화
        }

    }

    float GetSliderLengthInMs(HitSlider slider)
    {
        var pixelsPerBeat = GameManager.Instance.parsingManager.BeatmapData[parsingSongName].SliderMultiplier * currentTimingPoint.MsPerBeat;
        var sliderLengthInBeats = (slider.PixelLength * slider.Repeat) / pixelsPerBeat;
        return pixelsPerBeat * sliderLengthInBeats;
    }

    public void CrateEffect(int n) // Perfect = 1, Good = 2, Miss = 3
    {
        if (effects.Count < poolingMax)
        {
            effects.Add(Instantiate(hitEffectPrefab, hitEffectTransform));
        }
        else
        {
            effects[indexforEffectPooling].gameObject.SetActive(true);
        }

        NoteHitEffect nowEffect = effects[indexforEffectPooling].GetComponent<NoteHitEffect>();
        switch (n)
        {
            case 1:
                {
                    nowEffect.text.color = hitEffectColors.perfectColor;
                    GameManager.Instance.Combo += 2;
                    nowEffect.text.text = $"PERFECT X {GameManager.Instance.Combo}";

                    BossSceneManager.Instance.progressBar.value += 2;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);

                    break;
                }
            case 2:
                {
                    nowEffect.text.color = hitEffectColors.goodColor;
                    ++GameManager.Instance.Combo;
                    nowEffect.text.text = $"GOOD X {GameManager.Instance.Combo}";

                    BossSceneManager.Instance.progressBar.value++;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);

                    break;
                }
            case 3:
                {
                    nowEffect.text.color = hitEffectColors.missColor;
                    GameManager.Instance.Combo = 0;
                    nowEffect.text.text = "MISS";
                    
                    BossSceneManager.Instance.progressBar.value--;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerHitHash);
                    

                    break;
                }
        }

        nowEffect.transform.position = hitEffectTransform.position;

        {
            Sequence seq = DOTween.Sequence();
            seq.OnStart(() => {
                nowEffect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            });
            seq.Append(nowEffect.GetComponent<RectTransform>().DOAnchorPosY(50, 1).SetEase(Ease.InBack));
            seq.Join(nowEffect.text.DOFade(0, 1));
            seq.OnComplete(() => nowEffect.gameObject.SetActive(false));
        }

        indexforEffectPooling++;
        if (indexforEffectPooling == poolingMax)
        {
            indexforEffectPooling = 0; // indexforNotePooling가 최대치라면 다시 초기화
        }

    }

    private void DeleteNote(GameObject note)
    {
        note.SetActive(false);
        DOTween.Complete(note);


    }

    public void CheckNote()
    {
        if(notesforPooling.Count == 0) // 목록이 없으면 그냥 리턴
        {
            return;
        }

        var item = notesforPooling[noteCheckIndex].GetComponent<NoteScript>();

        if (!item.gameObject.activeSelf) // 만약 비활성화 되있다면 다음으로 넘어가서 재시작!
        {
            noteCheckIndex++;

            if (noteCheckIndex == poolingMax)
                noteCheckIndex = 0;
            
            CheckNote();
        }

        int hit = item.isHit(noteLine.transform.position);
        
        if ((hit % 4) > 0) // None이 아닌 경우
        {
            //Camera.main.transform.DOShakePosition(shakeTime, shakePower / (hit / 4));
            CrateEffect(hit);
            DeleteNote(item.gameObject);

            noteCheckIndex++; // 다음 노트를 검사하게 Index++;

            if (noteCheckIndex == poolingMax)
                noteCheckIndex = 0;
        }
    }

}
