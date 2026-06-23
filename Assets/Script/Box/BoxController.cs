using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{
    [Header("Animation")]
    public Animator boxAnimator;
    public string triggerName = "DongHop";
    public float thoiGianDongHop = 1f;

    [Header("UI Manager Liên Kết")]
    public GameplayUIManager uiManager;

    [Header("Lật hộp")]
    public float thoiGianLat = 0.3f;
    public float kichThuocHop = 1f;
    private bool dangLat = false;
    private float banKinhXoay;

    void Start()
    {
        banKinhXoay = kichThuocHop / 2f;
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

        if (uiManager != null)
        {
            uiManager.TriggerShowAllPopups();
        }

        dangLat = false;
    }

    public void NutSangTrai()
    {
        if (dangLat) return;

        Vector3 tamLat =
            transform.position +
            new Vector3(-banKinhXoay, -banKinhXoay, 0);

        StartCoroutine(
            QuyTrinhLat(tamLat, Vector3.forward)
        );
    }

    public void NutSangPhai()
    {
        if (dangLat) return;

        Vector3 tamLat =
            transform.position +
            new Vector3(banKinhXoay, -banKinhXoay, 0);

        StartCoroutine(
            QuyTrinhLat(tamLat, Vector3.back)
        );
    }

    public void NutTienLen()
    {
        if (dangLat) return;

        Vector3 tamLat =
            transform.position +
            new Vector3(0, -banKinhXoay, banKinhXoay);

        StartCoroutine(
            QuyTrinhLat(tamLat, Vector3.right)
        );
    }

    public void NutLuiLai()
    {
        if (dangLat) return;

        Vector3 tamLat =
            transform.position +
            new Vector3(0, -banKinhXoay, -banKinhXoay);

        StartCoroutine(
            QuyTrinhLat(tamLat, Vector3.left)
        );
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