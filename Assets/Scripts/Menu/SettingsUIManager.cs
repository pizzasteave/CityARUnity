using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SettingsUIManager : MonoBehaviour
{
    RectTransform rectTransform;

    #region Getter
    static SettingsUIManager instance;
    public static SettingsUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SettingsUIManager>();
            if (instance == null)
                Debug.LogError("HomeUIManager not found");
            return instance;
        }
    }
    #endregion Getter

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosY(-rectTransform.rect.height, 0f);

        this.gameObject.SetActive(false);
    }

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosY(0, 0.3f).SetDelay(delay);
    }

    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosY(-rectTransform.rect.height, 0.3f).SetDelay(delay);
        StartCoroutine(WaitHide());
    
    }

    private IEnumerator WaitHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }


    public void ShowHomeScreen()
    {
        Hide();
    }
}