using UnityEngine;
using DG.Tweening;

public class MissionPopup : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform popupPanel;    // Kéo thả Object 'Complete' vào đây
    public RectTransform btnContinue;   // Kéo thả nút tiếp tục (nếu có)

    private Vector3 origPanelScale;
    private Vector3 origBtnScale;
    private bool isTriggered = false;

    void Start()
    {
        // 1. Lưu lại Scale chuẩn (thường là 1, 1, 1) từ Inspector
        origPanelScale = popupPanel.localScale;
        if (btnContinue != null) origBtnScale = btnContinue.localScale;

        // 2. Thay vì chỉnh Scale về 0 làm tàng hình chữ, ta TẮT hẳn Object đi khi vào game
        popupPanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            ShowPopup();
        }
    }

    void ShowPopup()
    {
        // 1. Bật Object lên trước để Unity bắt đầu vẽ UI
        popupPanel.gameObject.SetActive(true);

        // 2. Đặt scale về 0 ngay trước khi chạy Tween để tạo hiệu ứng phóng to
        popupPanel.localScale = Vector3.zero;

        // 3. Thực hiện hiệu ứng phóng to nảy nhẹ (OutBack) về đúng kích thước chuẩn
        popupPanel.DOScale(origPanelScale, 0.5f).SetEase(Ease.OutBack);

        // Xử lý nút bấm đi kèm (nếu có)
        if (btnContinue != null)
        {
            btnContinue.localScale = Vector3.zero;
            btnContinue.DOScale(origBtnScale, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);
        }
    }

    public void ClosePopup()
    {
        popupPanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
            popupPanel.gameObject.SetActive(false); // Tắt hẳn khi thu nhỏ xong
            isTriggered = false;
        });
    }
}