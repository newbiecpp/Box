using UnityEngine;

public class QuestArrow : MonoBehaviour
{
    [Header("Mục tiêu theo dõi")]
    public Transform boxTransform;
    public Transform targetCircle;

    [Header("Cấu hình vị trí & Xoay")]
    public float heightOffset = 0.6f;
    public float rotationSpeed = 10f;

    void LateUpdate()
    {
        if (boxTransform == null || targetCircle == null) return;

        // 1. Giữ vị trí luôn dính trên mặt hộp
        Vector3 targetPosition = boxTransform.position + Vector3.up * heightOffset;
        transform.position = targetPosition;

        // 2. Tính toán hướng xoay về phía vòng tròn (chỉ xoay trên mặt phẳng ngang)
        Vector3 direction = targetCircle.position - transform.position;
        direction.y = 0; // Khóa trục Y để không bị chúi xuống

        if (direction != Vector3.zero)
        {
            // Vì Sprite của bạn đang nằm phẳng (X = -90), ta cần tính góc quay trên mặt phẳng phẳng
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Xoay quanh trục Y để chỉ hướng, nhưng vẫn giữ X = -90 để nằm phẳng trên mặt hộp
            Quaternion targetRotation = Quaternion.Euler(-90f, angle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}