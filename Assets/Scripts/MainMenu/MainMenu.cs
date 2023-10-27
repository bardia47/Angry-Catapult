using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public RectTransform buttonsHolder;
    public CanvasGroup buttonsCg;
    public float buttonsTransitionSpeed;

    public GameObject playButton;
    public GameObject firstLevelButton;
    public Button backToMenuSelectButton;

    [Header("Fader")]
    public Image fader;

    [Header("Main Menu")]
    public CanvasGroup mainMenuCg;
    public CanvasGroup levelSelectCg;
    public CanvasGroup optionsCg;

    public Transform levelsHolder;
    public static bool showLevelSelectOnLoad = false;

  
    [Header("Options")]
    public bool fullScreen;
    public int resolutionIndex;
    private Resolution[] resolutions;
    public TMP_Text resoText, fullscreenText;//, volumeText;


    [Header("Music")]
    public Image music;
    public Sprite musicOnSprite, musicOffSprite;


    private void Start()
    {
        if (ApplicationUtils.isPhoneMode())

        {
            foreach (var item in buttonsCg.GetComponentsInChildren<Button>())
            {
                if (!item.CompareTag("PlayButton"))
                {
                    item.transform.localScale = Vector3.zero;
                }
            }

        } else { 
        resolutions = Screen.resolutions; 
        }

        Sequence s = DOTween.Sequence();
        s.AppendInterval(0.5f);
        s.AppendCallback(() => {
            FadeIn();
            AudioManager.instance.backgroundMusic.Play();
        });
        s.AppendCallback(() =>
        {
            if (!showLevelSelectOnLoad)
            {
                buttonsCg.DOFade(1f, buttonsTransitionSpeed).SetDelay(1f);
            }
            else
            {
                levelSelectCg.DOFade(1f, buttonsTransitionSpeed).SetDelay(1f).OnComplete(() =>
                 {
                     levelSelectCg.interactable = levelSelectCg.alpha == 1f;
                     levelSelectCg.blocksRaycasts = levelSelectCg.alpha == 1f;
                     for (int i = 0; i < levelsHolder.childCount; i++)
                     {
                         levelsHolder.GetChild(i).GetComponent<GraphicRaycaster>().enabled = true;
                     }
                 }

                 
                );
            }

        });
        s.Play();

        for (int i = 0; i < levelsHolder.childCount; i++)
        {
            Navigation nav = levelsHolder.GetChild(i).GetComponent<Button>().navigation;
            nav.selectOnRight = i + 1 > levelsHolder.childCount - 1 ?
                null : levelsHolder.GetChild(i + 1).GetComponent<Button>();

            nav.selectOnLeft = i - 1 < 0 ?
                null : levelsHolder.GetChild(i - 1).GetComponent<Button>();

            if (levelsHolder.childCount > i + 5)
            {
                nav.selectOnDown = levelsHolder.GetChild(i + 5).GetComponent<Button>();
            }
            else
            {
                nav.selectOnDown = backToMenuSelectButton;
            }
            if (i > 5)
            {
                nav.selectOnUp = levelsHolder.GetChild(i - 5).GetComponent<Button>();
            }
            levelsHolder.GetChild(i).GetComponent<Button>().navigation = nav;
        }
        music.sprite = AudioListener.volume == 1f ? musicOnSprite : musicOffSprite;

    }


    public void FadeOut(string scene)
    {
        fader.raycastTarget = true;
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
        fader.raycastTarget = false;
        fader.DOFillAmount(0f, 0.3f).SetDelay(0.5f);
    }

    public void OnPlayGame() {

        Sequence s = DOTween.Sequence();
        s.Append(mainMenuCg.DOFade(0f, 0.5f).OnComplete(() =>
         {

             mainMenuCg.interactable = false;
             mainMenuCg.blocksRaycasts = false;
         }));

        s.Append(levelSelectCg.DOFade(1f, 0.5f).OnComplete(() =>
        {
            levelSelectCg.interactable = true;
            levelSelectCg.blocksRaycasts = true;
            for (int i = 0; i < levelsHolder.childCount; i++)
            {
                levelsHolder.GetChild(i).GetComponent<GraphicRaycaster>().enabled = true;
            }

        }
        ));
       
        s.Play();
       
    }

    public void OnBackToMainMenu()
    {

        Sequence s = DOTween.Sequence();
        s.Append(levelSelectCg.alpha == 1f ?
            levelSelectCg.DOFade(0f, 0.5f)
            .OnComplete(() =>
        {

            levelSelectCg.interactable = false;
            levelSelectCg.blocksRaycasts = false;
            for (int i = 0; i < levelsHolder.childCount; i++)
            {
                levelsHolder.GetChild(i).GetComponent<GraphicRaycaster>().enabled = false;
            }

        })
            : optionsCg.DOFade(0f, 0.5f).OnComplete(() =>
            {

                optionsCg.interactable = false;
                optionsCg.blocksRaycasts = false;
            })
        );

        s.Append(mainMenuCg.DOFade(1f, 0.5f)).OnComplete(() =>
        {
            buttonsCg.DOFade(1f, buttonsTransitionSpeed).OnComplete(() =>

             { 
                 buttonsCg.interactable = true;
                 buttonsCg.blocksRaycasts = true;
             }
             
             );
            mainMenuCg.interactable = true;
            mainMenuCg.blocksRaycasts = true;
        }
        );
        s.Play();

    }

    public void OnOptions() {
        Sequence s = DOTween.Sequence();
        s.Append(mainMenuCg.DOFade(0f, 0.5f)).OnComplete(() =>
         {

        mainMenuCg.interactable = false;
        mainMenuCg.blocksRaycasts = false;
    });

        s.Append(optionsCg.DOFade(1f, 0.5f)).OnComplete(() =>
        {
        optionsCg.interactable = true;
            optionsCg.blocksRaycasts = true;
    }
        );
        s.Play();
    
    }

    public void OnQuitGame() {
        fader.DOFillAmount(1f, 0.3f).OnComplete(()=> {
            Application.Quit();
        }
        );
    
    }


    public void OnResolution() {
        resolutionIndex++;

        if (resolutionIndex == resolutions.Length)
        {
            resolutionIndex = 0;
        }

        resoText.text = $"{resolutions[resolutionIndex].width} x {resolutions[resolutionIndex].height}";
    }

    public void OnFullscreen() {
        fullScreen = !fullScreen;
        fullscreenText.text = fullScreen ? "Enabled" : "Disabled";
    }
    public void OnVolume()
    {
        AudioListener.volume = AudioListener.volume == 1f ? 0f : 1f;
        //volumeText.text = AudioListener.volume==1f ? "On" : "Off";
        music.sprite = music.sprite == musicOnSprite ? musicOffSprite : musicOnSprite;

    }

    public void OnApplySettings() {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height,fullScreen);
        OnBackToMainMenu();
    }


    public void OnPlayLevel(int level)
    {
        FadeOut(GameManager.SCENE_LEVEL + (level + 1).ToString());
    }


}
