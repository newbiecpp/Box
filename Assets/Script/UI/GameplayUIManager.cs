using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameplayUIManager : MonoBehaviour
{
    [Header("Danh sách UI cần Popup")]
    public RectTransform[] uiPanels;

    [Header("Cấu hình hiệu ứng")]
    public float duration = 0.5f;
    public float delayBetween = 0.15f;

    public void PreHideAllUI()
    {
        foreach (RectTransform panel in uiPanels)
        {
            if (panel != null) panel.gameObject.SetActive(false);
        }
    }

    public void TriggerShowAllPopups()
    {
        StartCoroutine(StartUIPopupRoutine());
    }

    IEnumerator StartUIPopupRoutine()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < uiPanels.Length; i++)
        {
            if (uiPanels[i] == null) continue;

            RectTransform currentPanel = uiPanels[i];
            currentPanel.gameObject.SetActive(true);
            currentPanel.localScale = Vector3.zero;

            float currentDelay = i * delayBetween;

            currentPanel.DOScale(Vector3.one, duration)
                        .SetEase(Ease.OutBack)
                        .SetDelay(currentDelay);
        }
    }
}