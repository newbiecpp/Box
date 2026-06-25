using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraRotateController : MonoBehaviour
{
    [Header("Mục tiêu theo dõi")]
    public Transform box;

    [Header("Liên kết Camera")]
    public CinemachineCamera gameCam;
    public BoxController boxController;
    public float rotateDuration = 0.3f;

    private bool isRotating = false;
    private int currentYAngle = 0;
    private float cachedXAngle = 45f;
    private CinemachineFollow cmFollow;

    void Start()
    {
        if (gameCam != null)
        {
            cmFollow = gameCam.GetComponent<CinemachineFollow>();
        }

        cachedXAngle = transform.localEulerAngles.x;
        currentYAngle = Mathf.RoundToInt(transform.localEulerAngles.y);
    }

    void LateUpdate()
    {
        if (box != null)
            transform.position = box.position;
    }

    public void RotateLeft()
    {
        if (isRotating || cmFollow == null || boxController == null) return;

        currentYAngle -= 90;
        boxController.cameraDirection = (boxController.cameraDirection + 3) % 4;

        XoayVaCapNhatOffset(currentYAngle);
    }

    public void RotateRight()
    {
        if (isRotating || cmFollow == null || boxController == null) return;

        currentYAngle += 90;
        boxController.cameraDirection = (boxController.cameraDirection + 1) % 4;

        XoayVaCapNhatOffset(currentYAngle);
    }

    void XoayVaCapNhatOffset(int targetYAngle)
    {
        isRotating = true;

        Sequence rotateSequence = DOTween.Sequence();

        rotateSequence.Join(
            transform.DORotate(new Vector3(cachedXAngle, targetYAngle, 0f), rotateDuration)
                .SetEase(Ease.OutCubic)
        );

        Vector3 mocGocXa = boxController.offsetGocXa;
        Vector3 targetOffset = Quaternion.Euler(0, targetYAngle, 0) * mocGocXa;

        rotateSequence.Join(
            DOTween.To(() => cmFollow.FollowOffset, x => cmFollow.FollowOffset = x, targetOffset, rotateDuration)
                .SetEase(Ease.OutCubic)
        );

        rotateSequence.OnComplete(() => isRotating = false);
    }
}