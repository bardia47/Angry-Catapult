using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPrefab : MonoBehaviour
{
    private float lengthOfClip;
    private AudioSource a;
    private void Start()
    {
        a = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        lengthOfClip = 0f;
    }

    private void Update()
    {
        lengthOfClip += Time.deltaTime;
        if (!a.loop)
            if (lengthOfClip > a.clip.length + 0.5f)
                gameObject.SetActive(false);
    }
}
