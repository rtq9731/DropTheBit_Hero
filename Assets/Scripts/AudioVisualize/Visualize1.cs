using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualize1 : MonoBehaviour
{
    [SerializeField] private float shakePower;

    float startRange;
    private void Start()
    {
        startRange = gameObject.GetComponent<HardLight2D>().Range;
    }

    public void ListenToVolumeChange()
    {
        gameObject.GetComponent<HardLight2D>().Range = startRange * (AudioVisualizeManager.Output_Volume * shakePower + 1);
    }
}
