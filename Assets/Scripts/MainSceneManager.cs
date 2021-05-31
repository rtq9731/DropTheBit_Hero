using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    private void Awake()
    {
        Instance = this;    
    }
    void OnDestroy()
    {
        Instance = null;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void ScrollingBackground()
    {
        backGround.isScroll = !backGround.isScroll;
    }
}
