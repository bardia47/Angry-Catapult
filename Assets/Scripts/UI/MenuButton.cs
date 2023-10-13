using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler,IDeselectHandler
{

    public Image image;
    public float scale = 1.2f;

    public void OnScaleUp() {
        AudioManager.instance.CreateAndPlaySound(SoundClips.MENUHOVER, null, 0.3f, 1f);


        if (image != null)
            image.transform.DOScale(Vector3.one * scale, 0.2f).SetUpdate(true);
        else
            this.transform.DOScale(Vector3.one * scale, 0.2f).SetUpdate(true);
    }

    public void OnScaleDown()
    {
        if (image != null)
            image.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        else
            this.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
    }

    public void OnScalePress()
    {
        AudioManager.instance.CreateAndPlaySound(SoundClips.MENUCLICK, null, 0.5f, 1f);


        if (image != null)
            image.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 10, 0f).SetUpdate(true);
        else
            this.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 10, 0f).SetUpdate(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnScaleDown();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnScalePress();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnScaleUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnScaleDown();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnScaleUp();

    }
}
