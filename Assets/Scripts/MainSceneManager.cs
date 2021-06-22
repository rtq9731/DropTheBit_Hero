using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        Instance = null;
    }

    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;
    [SerializeField] public AudioSource hitSound;
    [SerializeField] public InputPanel InputPanel;
    [SerializeField] public LeftUI leftUI;

    public Player Player { get { return player; } set { player = value; } }

    public void CallBoss(int killCount)
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        leftUI.SetActiveTrueBtnBoss();
    }

    public void ScrollingBackground()
    {
        backGround.isScroll = !backGround.isScroll;
    }

    public void DoHit()
    {
        hitSound.Play();
    }
}
