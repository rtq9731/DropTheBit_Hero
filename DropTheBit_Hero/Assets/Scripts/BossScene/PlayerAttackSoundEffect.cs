using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSoundEffect : MonoBehaviour
{
    [SerializeField] AudioSource source;

    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        source.Play();
    }
}
