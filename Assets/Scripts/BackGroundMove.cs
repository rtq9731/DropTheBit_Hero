using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] List<Image> images;

    [SerializeField] List<float> backgroundSpeeds;

    public bool isScroll = true;

    List<float> vectorXList = new List<float>() { 0, 0, 0, 0};

    private void Update()
    {
        if(isScroll)
        {
            for (int i = 0; i < vectorXList.Count; i++)
            {
                Mathf.Repeat(vectorXList[i], 10);
            }

            for (int i = 0; i < images.Count; i++)
            {
                vectorXList[i] += backgroundSpeeds[i] * Time.deltaTime;
                images[i].material.mainTextureOffset = new Vector2(vectorXList[i], images[i].material.mainTextureOffset.y);
            }
        }
    }
}
