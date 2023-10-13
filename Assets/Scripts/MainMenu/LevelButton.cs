using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class LevelButton : MonoBehaviour,ISelectHandler,IDeselectHandler,IPointerEnterHandler,IPointerExitHandler
{
    public TMP_Text levelNum;
    public CanvasGroup starsCg;
    public Transform starsHolder;
    private Canvas c;


    private void Start()
    {
        c = GetComponent<Canvas>();
        levelNum.text = $"{transform.GetSiblingIndex() + 1}";
        if (transform.GetSiblingIndex() + 1 < SceneManager.sceneCountInBuildSettings)
        {

            this.GetComponent<Button>().onClick.AddListener(() =>
            {
                FindObjectOfType<MainMenu>().OnPlayLevel(transform.GetSiblingIndex());
            });
        }
        else this.gameObject.SetActive(false);
        int s = PlayerPrefs.GetInt($"{(transform.GetSiblingIndex() + 1).ToString()}",0);
        for (int i = 0; i < starsHolder.childCount; i++)
        {
            starsHolder.GetChild(i).gameObject.SetActive(i < s);
        }
        Hide();
    }

    public void Show() {
        c.sortingOrder = 2;
        levelNum.DOColor(Color.red, 0.2f);
        starsCg.DOFade(1f, 0.2f);
        starsHolder.localScale = Vector3.one;
        starsHolder.DOPunchScale(Vector3.one * 0.2f, 0.2f);
    }

    public void Hide() {
        c.sortingOrder = 1;
        levelNum.DOColor(Color.white, 0.2f);
        starsCg.DOFade(0f, 0.2f);
        starsHolder.localScale = Vector3.one;
        starsHolder.DOPunchScale(Vector3.one * -0.2f, 0.2f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Show();
    }
}
