using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{
    [Header("Animation")]
    public Animator boxAnimator;
    public string triggerName = "DongHop";
    public float thoiGianDongHop = 1f;

    [Header("Lật hộp")]
    public float thoiGianLat = 0.3f;
    public float kichThuocHop = 1f;

    [Header("Vuốt")]
    public float nguongVuot = 100f;

    private bool dangLat = false;
    private float banKinhXoay;
    private Vector2 diemBatDauVuot;

    void Start()
    {
        banKinhXoay = kichThuocHop / 2f;
        StartCoroutine(DongHopBanDau());
    }

    void Update()
    {
        XuLyVuotDienThoai();

#if UNITY_EDITOR
        XuLyVuotChuot();
#endif
    }

    IEnumerator DongHopBanDau()
    {
        dangLat = true;

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger(triggerName);
            yield return new WaitForSeconds(thoiGianDongHop);
        }

        dangLat = false;
    }

    void XuLyVuotDienThoai()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                diemBatDauVuot = touch.position;
                break;

            case TouchPhase.Ended:
                XuLyHuongVuot(touch.position - diemBatDauVuot);
                break;
        }
    }

    void XuLyVuotChuot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            diemBatDauVuot = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 doLech =
                (Vector2)Input.mousePosition - diemBatDauVuot;

            XuLyHuongVuot(doLech);
        }
    }

    void XuLyHuongVuot(Vector2 doLech)
    {
        if (doLech.magnitude < nguongVuot)
            return;

        if (Mathf.Abs(doLech.x) > Mathf.Abs(doLech.y))
        {
            if (doLech.x > 0)
                NutSangPhai();
            else
                NutSangTrai();
        }
        else
        {
            if (doLech.y > 0)
                NutTienLen();
            else
                NutLuiLai();
        }
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

    IEnumerator QuyTrinhLat(
        Vector3 tamLat,
        Vector3 trucXoay)
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
                gocMoiKhungHinh);

            gocDaXoay += gocMoiKhungHinh;

            yield return null;
        }

        dangLat = false;
    }
}