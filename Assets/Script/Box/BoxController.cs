using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using DG.Tweening;

public class BoxController : MonoBehaviour
{
    [Header("Animation")]
    public Animator boxAnimator;
    public string triggerName = "DongHop";
    public float thoiGianDongHop = 2.1f;

    [Header("Cinemachine Camera")]
    public CinemachineCamera introCam;
    public CinemachineCamera gameCam;
    public Vector3 offsetGocGan = new Vector3(0f, 1f, -3f);
    public Vector3 offsetGocXa = new Vector3(0f, 6f, -6f);
    public float thoiGianZoomOut = 1f;

    [HideInInspector]
    public int cameraDirection = 0;

    [Header("UI Manager Liên Kết")]
    public GameplayUIManager uiManager;

    [Header("Mũi tên nhiệm vụ (ArrowBox)")]
    public GameObject questArrow;

    [Header("Lật hộp")]
    public float thoiGianLat = 0.3f;
    public float kichThuocHop = 1f;
    private bool dangLat = false;
    private float banKinhXoay;

    [Header("Camera Hướng Dẫn")]
    public Transform cameraTransform;

    private CinemachineFollow cmFollow;

    void Start()
    {
        banKinhXoay = kichThuocHop / 2f;
        if (introCam != null)
        {
            cmFollow = introCam.GetComponent<CinemachineFollow>();
            if (cmFollow != null)
            {
                cmFollow.FollowOffset = offsetGocGan;
            }
        }
        if (questArrow != null)
        {
            questArrow.SetActive(false);
        }
        if (uiManager != null)
        {
            uiManager.PreHideAllUI();
        }
        StartCoroutine(DongHopBanDau());
    }

    IEnumerator DongHopBanDau()
    {
        dangLat = true;

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger(triggerName);
            yield return new WaitForSeconds(thoiGianDongHop);
        }

        yield return new WaitForEndOfFrame();

        if (cmFollow != null)
        {
            DOTween.To(() => cmFollow.FollowOffset, x => cmFollow.FollowOffset = x, offsetGocXa, thoiGianZoomOut)
                   .SetEase(Ease.OutCubic)
                   .OnUpdate(() => {
                       if (uiManager != null) uiManager.PreHideAllUI();
                       if (questArrow != null) questArrow.SetActive(false);
                   })
                   .OnComplete(() =>
                   {
                       if (introCam != null)
                           introCam.Priority = 0;

                       if (gameCam != null)
                           gameCam.Priority = 100;

                       if (questArrow != null)
                       {
                           questArrow.SetActive(true);
                           Vector3 targetScale = questArrow.transform.localScale;
                           questArrow.transform.localScale = Vector3.zero;
                           questArrow.transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack);
                       }

                       if (uiManager != null)
                       {
                           uiManager.TriggerShowAllPopups();
                       }

                       dangLat = false;
                   });
        }
        else
        {
            if (questArrow != null) questArrow.SetActive(true);
            if (uiManager != null) uiManager.TriggerShowAllPopups();
            dangLat = false;
        }
    }

    public void NutTienLen()
    {
        if (dangLat) return;
        TinhToanVaLat(GetCameraForward());
    }

    public void NutLuiLai()
    {
        if (dangLat) return;
        TinhToanVaLat(-GetCameraForward());
    }

    public void NutSangTrai()
    {
        if (dangLat) return;
        TinhToanVaLat(-GetCameraRight());
    }

    public void NutSangPhai()
    {
        if (dangLat) return;
        TinhToanVaLat(GetCameraRight());
    }

    // Lấy hướng thẳng tới của camera quy về mặt phẳng ngang (bỏ qua trục Y)
    Vector3 GetCameraForward()
    {
        if (cameraTransform == null) return Vector3.forward;
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    // Lấy hướng sang phải của camera quy về mặt phẳng ngang
    Vector3 GetCameraRight()
    {
        if (cameraTransform == null) return Vector3.right;
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right.normalized;
    }

    // Hàm tự động tính toán tâm lật và trục xoay dựa trên hướng di chuyển mong muốn
    void TinhToanVaLat(Vector3 huongDiChuyen)
    {
        // Làm tròn hướng di chuyển về các trục vuông góc gần nhất (0, 1, hoặc -1) để tránh sai số khi xoay camera lệch
        huongDiChuyen.x = Mathf.Round(huongDiChuyen.x);
        huongDiChuyen.z = Mathf.Round(huongDiChuyen.z);

        // Tâm lật nằm ở cạnh dưới của hộp theo hướng di chuyển
        Vector3 tamLat = transform.position + new Vector3(huongDiChuyen.x * banKinhXoay, -banKinhXoay, huongDiChuyen.z * banKinhXoay);

        // Trục xoay sẽ vuông góc với hướng di chuyển (Quy tắc bàn tay phải)
        Vector3 trucXoay = Vector3.Cross(Vector3.up, huongDiChuyen).normalized;

        StartCoroutine(QuyTrinhLat(tamLat, trucXoay));
    }

    IEnumerator QuyTrinhLat(Vector3 tamLat, Vector3 trucXoay)
    {
        dangLat = true;

        float gocDaXoay = 0f;

        while (gocDaXoay < 90f)
        {
            float gocMoiKhungHinh =
                (90f / thoiGianLat) * Time.deltaTime;

            if (gocDaXoay + gocMoiKhungHinh > 90f)
            {
                gocMoiKhungHinh =
                    90f - gocDaXoay;
            }

            transform.RotateAround(
                tamLat,
                trucXoay,
                gocMoiKhungHinh
            );

            gocDaXoay += gocMoiKhungHinh;

            yield return null;
        }

        dangLat = false;
    }
}