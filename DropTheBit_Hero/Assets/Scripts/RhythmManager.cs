using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class RhythmManager : MonoBehaviour
{
    class HitEffectColors
    {
        public Color perfectColor = new Color(1, 0.8874891f, 0, 1);
        public Color goodColor = new Color(0.6792453f, 0.9150943f, 0.06030021f, 1);
        public Color missColor = new Color(0.6792453f, 0, 0.06030021f, 1);
    }

    [SerializeField] Text debugText;

    [Header("오디오 관련")]
    [SerializeField] AudioSource audioSource;

    [Header("노트 관련")]
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject longNotePrefab;
    [SerializeField] Transform noteMakeTr;
    [SerializeField] public GameObject noteLine;

    private Queue<GameObject> noteObjPool = new Queue<GameObject>();
    private Queue<NoteScript> noteCheckPool = new Queue<NoteScript>();
    private Queue<GameObject> hitEffectPool = new Queue<GameObject>();
    public Queue<GameObject> textEffectPool = new Queue<GameObject>();

    private int noteMakeIndex = 0;
    private int hitCount = 0;

    private int currentTimingPointIndex = 0;

    TimingPoint currentTimingPoint = new TimingPoint();

    private float noteTimer = 0;
    private float noteLineDistance = 0f;
    private bool isTimingPointPlay = false;

    [Header("콤보 이펙트 관련")]
    [SerializeField] Transform textEffectPoolObj;
    [SerializeField] Transform hitEffectPoolObj;
    [SerializeField] GameObject textEffectPrefab;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] Transform cameraPos;
    [SerializeField] float cameraShakePower = 5f;
    [SerializeField] float cameraShakeTime = 0.1f;

    private HitEffectColors textEffectColors = new HitEffectColors();

    private bool isPlayingNote = false;

    private void Start()
    {
        StartStopSong();
    }

    private void Update()
    { 
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Delete))
            StartCoroutine(FinishRhythm());
#endif

        if(isTimingPointPlay)
        {
            if(GameManager.Instance.GetCurrentBeatmap().TimingPoints.Count > 0)
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
            else
            {
                currentTimingPoint = new TimingPoint();
                currentTimingPoint.MsPerBeat = 0f;
                isTimingPointPlay = false;
            }
        }


        if (isPlayingNote)
        {

            noteLineDistance = Vector2.Distance(noteMakeTr.position, noteLine.transform.position);

            noteTimer += Time.deltaTime * 1000;

            if(GameManager.Instance.GetVirtualBeatmaps().timings.Count <= noteMakeIndex)
            {
                StartCoroutine(FinishRhythm());
                return;
            }

            BossSceneManager.Instance.progressBar.maxValue = GameManager.Instance.GetVirtualBeatmaps().timings.Count;
            float noteTiming = GameManager.Instance.GetVirtualBeatmaps().timings[noteMakeIndex];
            if (isPlayingNote && noteTimer >= noteTiming - 1000 - currentTimingPoint.MsPerBeat) // 노트 타격지점 까지 1초가 걸리도록 설계해놓음. 그래서 1초 + 오프셋 빼줄 것임.
            {
                //Debug.Log(GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex].GetType().Name);
                //if ((GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex].GetType().Name == "HitSlider"))// 롱노트를 만들도록 해야함
                //{
                //    ++noteMakeIndex;
                //    CrateNote();
                //    //noteMakeIndex++;
                //    //CreateLongNote((HitSlider)GameManager.Instance.GetCurrentBeatmap().HitObjects[noteMakeIndex]);
                //}
                //else // 일반 노트
                //{
                    ++noteMakeIndex;
                    CrateNote();
                //}
            }

        }

        if(Input.GetMouseButtonDown(0))
        {
            MouseClickEffect();
            if (noteCheckPool.Count > 0)
            {
                CheckNote();
            }
        }
    }

    public void StartStopSong()
    {
        if (!isPlayingNote)
            audioSource.clip = GameManager.Instance.GetMusic();

        isPlayingNote = !isPlayingNote;
        isTimingPointPlay = !isTimingPointPlay;

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
        yield return new WaitForSeconds(2f); // 마지막 노트가 지나갈때까지 대기.
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

    public float GetRatePercent()
    {
        return (float)hitCount / (float)GameManager.Instance.GetVirtualBeatmaps().timings.Count * (float)100;
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
    //    longNote.InitLongNote(noteLine.transform.position, noteLineDistance, longNoteLength, noteMakeTr.position); // 노트 초기화

    //    longNoteMakeIndex++;
    //    if (longNoteMakeIndex == longNotePoolingMax)
    //    {
    //        longNoteMakeIndex = 0; // longNoteMakeIndex가 최대치라면 다시 초기화
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

        if(n % 3 > 0)
        {
            CrateHitEffect();
        }

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
        TextEffect currentEffect = current.GetComponent<TextEffect>();
        switch (n)
        {
            case 1:
                {
                    currentEffect.text.color = textEffectColors.perfectColor;
                    GameManager.Instance.Combo += 2;
                    currentEffect.text.text = $"PERFECT X {GameManager.Instance.Combo}";
                    hitCount++;

                    BossSceneManager.Instance.progressBar.value += 2;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);

                    break;
                }
            case 2:
                {
                    currentEffect.text.color = textEffectColors.goodColor;
                    ++GameManager.Instance.Combo;
                    currentEffect.text.text = $"GOOD X {GameManager.Instance.Combo}";
                    hitCount++;

                    BossSceneManager.Instance.progressBar.value++;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);

                    break;
                }
            case 3:
                {
                    currentEffect.text.color = textEffectColors.missColor;
                    GameManager.Instance.Combo = 0;
                    currentEffect.text.text = "MISS";
                    
                    BossSceneManager.Instance.progressBar.value--;
                    BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerHitHash);
                    

                    break;
                }
        }

        currentEffect.transform.position = textEffectPoolObj.position;

        {
            Sequence seq = DOTween.Sequence();
            seq.OnStart(() => {
                currentEffect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            });
            seq.Append(currentEffect.GetComponent<RectTransform>().DOAnchorPosY(50, 1).SetEase(Ease.InBack));
            seq.Join(currentEffect.text.DOFade(0, 1));
            seq.OnComplete(() => currentEffect.gameObject.SetActive(false));
        }

        textEffectPool.Enqueue(current);
    }

    void CrateHitEffect()
    {
        GameObject current;
        if (textEffectPool.Count > 0)
        {
            if (textEffectPool.Peek().activeSelf)
            {
                current = Instantiate(hitEffectPrefab, hitEffectPoolObj);
            }
            else
            {
                current = hitEffectPool.Dequeue();
            }
        }
        else
        {
            current = Instantiate(hitEffectPrefab, hitEffectPoolObj);
        }

        current.gameObject.SetActive(true);
        ParticleSystem currentParticle = current.GetComponent<ParticleSystem>();
        StartCoroutine(PlayHitEffect(currentParticle));
        hitEffectPool.Enqueue(current.gameObject);
    }

    private IEnumerator PlayHitEffect(ParticleSystem particle)
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(noteLine.transform.position);
        particle.transform.position = new Vector3(currentPos.x, currentPos.y, -5);
        particle.Play();
        yield return new WaitForSeconds(particle.main.duration);
        particle.gameObject.SetActive(false);
    }


    void DeleteNote(NoteScript note)
    {
        note.gameObject.SetActive(false);
        DOTween.Complete(note.gameObject);
    }

    void MouseClickEffect()
    {
        noteLine.transform.DOScale(1.2f, 0.1f).OnComplete(() => noteLine.transform.DOScale(1f, 0.1f));
    }

    public void CheckNote()
    {
        NoteScript currentNote = noteCheckPool.Peek();
        if (currentNote.isHit(noteLine.transform.position) % 4 > 0)
        {
            cameraPos.DOShakePosition(cameraShakeTime, cameraShakePower);
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
        //        if ((item.GetComponent<NoteScript>().isHit(noteLine.transform.position) % 4) > 0) // None이 아닌 경우
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
