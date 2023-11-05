using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClips
{
    MENUHOVER,//
    MENUCLICK,//
    GOSTAR//
    , STARPING,
    CATAPULT//
    , WOODBREAK,//
    THUD//
     , POOF//
    , VICTORY,
    DEFEAT
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip menuHover, menuClick, goStar, starPing, catapult,
        woodBreak, thud, poof, victory, defeat;
    public Dictionary<SoundClips, AudioClip> soundDictionary
        = new Dictionary<SoundClips, AudioClip>();

    public GameObject audioPrefab;
    public AudioSource backgroundMusic;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        soundDictionary.Add(SoundClips.MENUCLICK, menuClick);
        soundDictionary.Add(SoundClips.MENUHOVER, menuHover);
        soundDictionary.Add(SoundClips.GOSTAR, goStar);
        soundDictionary.Add(SoundClips.STARPING, starPing);
        soundDictionary.Add(SoundClips.CATAPULT, catapult);
        soundDictionary.Add(SoundClips.WOODBREAK, woodBreak);
        soundDictionary.Add(SoundClips.THUD, thud);
        soundDictionary.Add(SoundClips.POOF, poof);
        soundDictionary.Add(SoundClips.VICTORY, victory);
        soundDictionary.Add(SoundClips.DEFEAT, defeat);
        SetUpAudioPool(20);
    }

    private void SetUpAudioPool(int poolers)
    {
        for (int i = 0; i < poolers; i++)
        {
            AudioSource a = Instantiate(audioPrefab).GetComponent<AudioSource>();
            a.transform.SetParent(this.transform);
            a.loop = false;
            a.clip = null;
            a.gameObject.SetActive(false);
            a.playOnAwake = true;
        }


    }

    private GameObject GetPooler()
    {

        int num = 0;
        for (int i = 0; i < this.transform.childCount; i++)
            if (!this.transform.GetChild(i).gameObject.activeInHierarchy)
                num++;
        GameObject g = null;
        if (num <= 0)
            g = Instantiate(audioPrefab, this.transform);
        GameObject newG = default;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (!this.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                newG = this.transform.GetChild(i).gameObject;
                break;
            }
        }

        if (num <= 0)
        {
            return g;
        }
        else
        {
            newG.transform.SetAsLastSibling();
            newG.SetActive(true);
            return newG;
        }

    }
    public void RecyclePooler(GameObject g)
    {

        AudioSource a = g.GetComponent<AudioSource>();
        a.clip = null;
        g.SetActive(false);
        if (g.transform.parent != this.transform)
        {
            g.transform.SetParent(this.transform);
        }
    }

    public void CreateAndPlaySound(SoundClips sound, Vector3? pos = null, float volume = 1f, float pitch = 1f, bool audioPitchBend = true,
        bool loop = false, Transform parentAttach = null, float delay = 0f, bool space3d = true)
    {
        GameObject newAudio = GetPooler();
        if (!pos.HasValue)
        {
            pos = Camera.main.transform.position;
        }
        else {
            newAudio.transform.position = new Vector3(pos.Value.x, pos.Value.y, Camera.main.transform.position.z);

        }

        AudioSource a = newAudio.GetComponent<AudioSource>();
        a.clip = soundDictionary[sound];
        if (a.clip == null)
            return;

        a.spatialBlend = space3d ? 1 : 0;
        a.loop = loop;
        a.volume = volume;
        if (audioPitchBend)
        {
            a.pitch = Random.Range(pitch - 0.15f, pitch + 0.15f);
        }
        else a.pitch = pitch;

        if (parentAttach != null)
            a.transform.SetParent(parentAttach);

        a.Play((ulong)delay * 44100);
    }
}
