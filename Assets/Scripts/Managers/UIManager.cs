using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("In Game")]
    public Slider angleSlider;
    public Slider powerSlider;

    public TMP_Text angleNum;
    public TMP_Text powerNum;
    public TMP_Text inGameScore;

    public Image[] inGameStars;
    public Transform cannonballHolder;

    [Header("Canvas Groups")]
    public CanvasGroup catapultBarsCg;
    public CanvasGroup pauseCg;
    public CanvasGroup victoryCg;

    [Space]
    public Image fader;

    [Header("Pause")]
    public Image musicPause;
    public Sprite musicOnSprite, musicOffSprite;
   // public Image[] pauseStars;

    [Header("Victory")]
  //  public Image musicEnd;
    public Image[] victoryStars;
    public TMP_Text victoryScore;
    public TMP_Text winStatusTitleText;
    public Button nextLevelButton;



    [Header("Background")]
   // public float moveHorizontal ;
   // public float offsetHorizontal ;
    public Transform background;

    [Header("enemyScore")]
    public TMP_Text enemyScore;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }
    private void Start()
    {
        if (ApplicationUtils.isPhoneMode())
        {
            powerNum.transform.localScale = Vector3.zero;
            angleNum.transform.localScale = Vector3.zero;
        }

        musicPause.sprite = AudioListener.volume == 1f ? musicOnSprite : musicOffSprite;

        setPreLanchPosition();
        FadeIn();
        UpdateStarsThreshHolds();
    }

    public void ShowEnemyScore(Transform enemy,int points)
    {
        TMP_Text text =  Instantiate(enemyScore, enemy.position,Quaternion.identity, GetComponentsInChildren<Canvas>()[0].transform);
        text.text = points.ToString();
        text.transform.DOScale(1f, 1f);
        text.transform.DOMoveY(3, 1f).OnComplete(()=> text.DOFade(0,1f).OnComplete(()=>Destroy(text)));
        //  text.DOFade(0f, 2f).OnComplete(() => Destroy(text));


    }

    private void Update()
    {
        if (Application.platform!=RuntimePlatform.Android && Input.GetButtonDown("Pause"))
        {
            TogglePauseMenu();
        }
    }

    private void LateUpdate()
    {
        background.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, background.position.z);
    }

    public void UpdateAngle()
    {
        if (BallManager.instance.catapult == null)
            BallManager.instance.catapult = FindObjectOfType<Catapult>();
        BallManager.instance.catapult.CurrentRotate = angleSlider.value;
        angleNum.text = $"{((BallManager.instance.catapult.maxRotate - BallManager.instance.catapult.CurrentRotate) / (BallManager.instance.catapult.maxRotate - BallManager.instance.catapult.minRotate) * 100).ToString("F0")}";
    }

    public void UpdatePower()
    {
        if (BallManager.instance.catapult == null)
            BallManager.instance.catapult = FindObjectOfType<Catapult>();
        BallManager.instance.catapult.forcePower = powerSlider.value;
        powerNum.text = $"{((BallManager.instance.catapult.forcePower - BallManager.instance.catapult.minForce) / (BallManager.instance.catapult.maxForce - BallManager.instance.catapult.minForce) * 100).ToString("F0")}";
    }

    public void LaunchCatapult()
    {
        if (BallManager.instance.catapult == null)
            BallManager.instance.catapult = FindObjectOfType<Catapult>();
        BallManager.instance.catapult.LaunchCatapult();
    }

    public void UpdateBallsLeft()
    {

        for (int i = 0; i < cannonballHolder.childCount; i++)
        {
            cannonballHolder.GetChild(i).gameObject.SetActive(i < BallManager.instance.ballsLeft);
        }
    }

    public void UpdateStarsThreshHolds()
    {

        inGameScore.text = ScoreManager.instance.Score.ToString("0000");

        if (ScoreManager.instance.Score >= ScoreManager.instance.thresholdBronze)
        {
            if (inGameStars[0].transform.localScale.x==0f)
                AudioManager.instance.CreateAndPlaySound(SoundClips.STARPING, null, 0.5f, 1f);



            inGameStars[0].transform.DOScale(1f, 0.2f);
            //pauseStars[0].transform.localScale = Vector3.one;

        }
        if (ScoreManager.instance.Score >= ScoreManager.instance.thresholdSilver)
        {
            if (inGameStars[1].transform.localScale.x == 0f)
                AudioManager.instance.CreateAndPlaySound(SoundClips.STARPING, null, 0.5f, 1f);


            inGameStars[1].transform.DOScale(1f, 0.2f);
           // pauseStars[1].transform.localScale = Vector3.one;

        }
        if (ScoreManager.instance.Score >= ScoreManager.instance.thresholdGold)
        {

            if (inGameStars[2].transform.localScale.x == 0f)
                AudioManager.instance.CreateAndPlaySound(SoundClips.STARPING, null, 0.5f, 1f);


            inGameStars[2].transform.DOScale(1f, 0.2f);
          //  pauseStars[2].transform.localScale = Vector3.one;

        }

    }

    public void ToggleCatapultBars()
    {
        catapultBarsCg.DOFade(catapultBarsCg.alpha == 1f ? 0f : 1f, 0.2f).OnComplete(() =>
        {
            catapultBarsCg.blocksRaycasts = catapultBarsCg.alpha == 1f;
        });
    }

    public void TogglePauseMenu()
    {
        GameManager.ChangeState(pauseCg.alpha == 1f ? GameStates.INGAME : GameStates.PAUSE);
        pauseCg.transform.DOPunchScale(pauseCg.alpha == 1f ? Vector3.one * 0.2f
            : Vector3.one * -0.2f, 0.2f).SetUpdate(true);

        pauseCg.DOFade(pauseCg.alpha == 1f ? 0f : 1f, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            pauseCg.blocksRaycasts = pauseCg.alpha == 1f;
            pauseCg.interactable = pauseCg.alpha == 1f;

        }

        );


    }

    public void OnAudioTogglePause()
    {
        AudioListener.volume = AudioListener.volume == 1f ? 0f : 1f;
        musicPause.sprite = musicPause.sprite == musicOnSprite ? musicOffSprite : musicOnSprite;
    }

    public void OnLevelSelectTogglePause() {
        GameManager.ChangeState(GameStates.END);
        FadeOut(GameManager.SCENE_MENU);
    }

    /* public void OnAudioToggleEnd()
     {
         AudioListener.volume = AudioListener.volume == 1f ? 0f : 1f;
         musicEnd.sprite = musicEnd.sprite == musicOnSprite ? musicOffSprite : musicOnSprite;
     }*/

    public void ToggleEndScreen(bool win)
    {

        if (GameManager.instance.currentState == GameStates.END)
            return;
        GameManager.ChangeState(GameStates.END);
        winStatusTitleText.text = win ? "VICTORY" : "DEFEAT";

        AudioManager.instance.CreateAndPlaySound(win?SoundClips.VICTORY : SoundClips.DEFEAT, null, 1f, 1f,false);


        if (win)
        {
            nextLevelButton.onClick.AddListener(OnNextLevel);

        }
        else
        {
            nextLevelButton.onClick.AddListener(OnRestartLevel);
        }


        victoryCg.transform.DOPunchScale(victoryCg.alpha == 1f ? Vector3.one * 0.2f : Vector3.one * -0.2f, 0.2f)
            .SetUpdate(true);
        victoryCg.DOFade(victoryCg.alpha == 1f ? 0f : 1f, 0.2f).SetUpdate(true)
            .OnComplete(() =>
            {
                victoryCg.blocksRaycasts = victoryCg.alpha == 1f;
                victoryCg.interactable = victoryCg.alpha == 1f;

               

            });
        ShowStars();

    }

    private void ShowStars()
    {
        StartCoroutine(UpdateVictoryScore());//(ScoreManager.instance.thresholdBronze));

        if (ScoreManager.instance.isAboveThresholds)
        {
            victoryStars[0].transform.DOScale(1f, 0.2f).SetUpdate(true)
                .SetDelay(0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {

                    if (PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().buildIndex}") < 1)
                        PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().buildIndex}", 1);
                    AudioManager.instance.CreateAndPlaySound(SoundClips.GOSTAR, null, 0.7f, 1f);

                    if (ScoreManager.instance.Score >= ScoreManager.instance.thresholdSilver)
                    {
                        victoryStars[1].transform.DOScale(1f, 0.2f).SetUpdate(true)
        .SetEase(Ease.OutBack).OnComplete(() =>
        {

            if (PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().buildIndex}") < 2)
                PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().buildIndex}", 2);

            AudioManager.instance.CreateAndPlaySound(SoundClips.GOSTAR, null, 0.7f, 1f);


            if (ScoreManager.instance.Score >= ScoreManager.instance.thresholdGold)
            {

                victoryStars[2].transform.DOScale(1f, 0.2f)
                .SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    if (PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().buildIndex}") < 3)
                        PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().buildIndex}", 3);
                    AudioManager.instance.CreateAndPlaySound(SoundClips.GOSTAR, null, 0.7f, 1f);

                });

            }
        }
                        );
                    }
                }
                );
        }
    }

    private IEnumerator UpdateVictoryScore()//(int threshold)
    {
        double waitTime = 1f / (ScoreManager.instance.Score*5);
        for (int i = int.Parse(victoryScore.text); i <= (/*ScoreManager.instance.Score > threshold ? threshold :*/ ScoreManager.instance.Score); i++)
        {
            yield return new WaitForSeconds(Convert.ToSingle(waitTime));

            victoryScore.text = i.ToString("0000");
        }
    }

    public void FadeOut(string scene)
    {
        Sequence s = DOTween.Sequence();
        s.Append(fader.DOFillAmount(1f, 0.3f));
        s.AppendInterval(0.5f);
        s.AppendCallback(() =>
        {
            SceneManager.LoadScene(scene);
        });
    }
    public void FadeIn()
    {
        fader.DOFillAmount(0f, 0.3f).SetDelay(0.5f);
    }
    private void setPreLanchPosition()
    {
        catapultBarsCg.transform.position = FindObjectOfType<Catapult>().transform.position;
    }
  


    public void OnRestartLevel()
    {
        FadeOut(SceneManager.GetActiveScene().name);
    }

    public void OnNextLevel()
    {
        List<string> scenesInBuild = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
        char[] s = SceneManager.GetActiveScene().name.ToCharArray(5, SceneManager.GetActiveScene().name.Length - 5);
        int level = 0;
        string sceneNum = "";
        for (int i = 0; i < s.Length; i++)
        {
            sceneNum += s[i].ToString();
        }
        int.TryParse(sceneNum,out level);
        if (scenesInBuild.Contains(GameManager.SCENE_LEVEL + (level + 1)))
        {
            FadeOut(GameManager.SCENE_LEVEL + (level + 1));
        }
        else
            FadeOut(GameManager.SCENE_MENU);
    }

    public void OnLevelSelect()
    {
        MainMenu.showLevelSelectOnLoad = true;
        FadeOut(GameManager.SCENE_MENU);
    }
}