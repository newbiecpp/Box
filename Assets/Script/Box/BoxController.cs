using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{
    [Header("Cấu hình Animation")]
    public Animator boxAnimator;
    public string triggerName = "DongHop";
    public string animationStateName = "Armature|ArmatureAction";

    [Header("Cấu hình Lật")]
    public float thoiGianLat = 0.3f;
    public float kichThuocHop = 1f;

    private bool dangLat = false;
    private bool daDongHop = false;
    private float banKinhXoay;

    void Start()
    {
        banKinhXoay = kichThuocHop / 2f;
    }

    public void NutSangTrai()
    {
        if (dangLat) return;

        // Tâm lật nằm ở cạnh dưới bên trái (trục X giảm)
        Vector3 tamLat = transform.position + new Vector3(-banKinhXoay, -banKinhXoay, 0);
        Vector3 trucXoay = Vector3.forward; // Quay quanh trục Z để sang trái

        StartCoroutine(QuyTrinhXuLy(tamLat, trucXoay));
    }

    // --- 2. HÀM CHO NÚT RIGHT (SANG PHẢI) ---
    public void NutSangPhai()
    {
        if (dangLat) return;

        // Tâm lật nằm ở cạnh dưới bên phải (trục X tăng)
        Vector3 tamLat = transform.position + new Vector3(banKinhXoay, -banKinhXoay, 0);
        Vector3 trucXoay = Vector3.back; // Quay ngược quanh trục Z để sang phải

        StartCoroutine(QuyTrinhXuLy(tamLat, trucXoay));
    }

    // --- 3. HÀM CHO NÚT UP (TIẾN LÊN) ---
    public void NutTienLen()
    {
        if (dangLat) return;

        // Tâm lật nằm ở cạnh dưới phía trước (trục Z tăng)
        Vector3 tamLat = transform.position + new Vector3(0, -banKinhXoay, banKinhXoay);
        Vector3 trucXoay = Vector3.right; // Quay quanh trục X để lật tiến lên

        StartCoroutine(QuyTrinhXuLy(tamLat, trucXoay));
    }

    // --- 4. HÀM CHO NÚT DOWN (LÙI LẠI) ---
    public void NutLuiLai()
    {
        if (dangLat) return;

        // Tâm lật nằm ở cạnh dưới phía sau (trục Z giảm)
        Vector3 tamLat = transform.position + new Vector3(0, -banKinhXoay, -banKinhXoay);
        Vector3 trucXoay = Vector3.left; // Quay ngược quanh trục X để lật lùi lại

        StartCoroutine(QuyTrinhXuLy(tamLat, trucXoay));
    }

    // --- QUY TRÌNH KẾT HỢP ANIMATION VÀ LẬT ---
    IEnumerator QuyTrinhXuLy(Vector3 tamLat, Vector3 trucXoay)
    {
        dangLat = true; // Khóa nút

        // Nếu hộp chưa đóng: Chạy animation đóng hộp trước
        if (!daDongHop)
        {
            if (boxAnimator != null)
            {
                boxAnimator.SetTrigger(triggerName);

                // Chờ một chút để Animator kịp chuyển trạng thái
                yield return new WaitForSeconds(0.1f);

                float thoiGianAnimation = 1.0f;
                AnimatorStateInfo stateInfo = boxAnimator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName(animationStateName))
                {
                    thoiGianAnimation = stateInfo.length;
                }

                // Chờ cho đến khi hộp đóng nắp hoàn toàn
                yield return new WaitForSeconds(thoiGianAnimation);
            }

            daDongHop = true; // Đánh dấu đã đóng hộp
        }

        // THỰC HIỆN XOAY LẬT 90 ĐỘ
        float gocDaXoay = 0f;
        while (gocDaXoay < 90f)
        {
            float gocTrongKhungHinh = (90f / thoiGianLat) * Time.deltaTime;

            if (gocDaXoay + gocTrongKhungHinh > 90f)
            {
                gocTrongKhungHinh = 90f - gocDaXoay;
            }

            transform.RotateAround(tamLat, trucXoay, gocTrongKhungHinh);
            gocDaXoay += gocTrongKhungHinh;

            yield return null;
        }

        dangLat = false; // Mở khóa nút
    }
}