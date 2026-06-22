using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform logoRect;       // Kéo thả Logo (Scale 3.61) vào đây

    [Header("Buttons List")]
    public RectTransform btnLevel;       // Kéo thả nút Level vào đây
    public RectTransform btnPlay;        // Kéo thả nút Play vào đây
    public RectTransform btnSetting;     // Kéo thả nút Setting vào đây

    // Biến lưu scale gốc của từng nút
    private Vector3 originalLogoScale;
    private Vector3 origLevelScale;
    private Vector3 origPlayScale;
    private Vector3 origSettingScale;

    void Start()
    {
        // 1. Lưu lại Scale gốc được cấu hình từ Inspector
        originalLogoScale = logoRect.localScale;
        origLevelScale = btnLevel.localScale;
        origPlayScale = btnPlay.localScale;
        origSettingScale = btnSetting.localScale;

        // 2. Đặt tất cả scale về 0 để ẩn đi lúc vừa vào game
        logoRect.localScale = Vector3.zero;
        btnLevel.localScale = Vector3.zero;
        btnPlay.localScale = Vector3.zero;
        btnSetting.localScale = Vector3.zero;

        // 3. Chạy hiệu ứng phóng to (Popup) nảy nhẹ
        // Logo xuất hiện đầu tiên
        logoRect.DOScale(originalLogoScale, 0.5f).SetEase(Ease.OutBack);

        // 3 nút bấm xuất hiện nối đuôi nhau (Delay tăng dần: 0.2s -> 0.3s -> 0.4s)
        btnLevel.DOScale(origLevelScale, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);
        btnPlay.DOScale(origPlayScale, 0.5f).SetEase(Ease.OutBack).SetDelay(0.3f);
        btnSetting.DOScale(origSettingScale, 0.5f).SetEase(Ease.OutBack).SetDelay(0.4f);
    }

    public void GoToStartGame()
    {
        // Khi bấm chuyển scene, tất cả đồng loạt thu nhỏ lại rồi chuyển cảnh
        btnLevel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        btnPlay.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        btnSetting.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

        logoRect.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            SceneManager.LoadScene("StartGame");
        });
    }
}