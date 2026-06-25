using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    public CameraRotateController cameraController;
    public float swipeThreshold = 80f;

    private Vector2 startPos;
    private bool isSwiping = false;

    void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f)
                return;

            startPos = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 endPos = Input.mousePosition;
            CheckSwipe(endPos);
            isSwiping = false;
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (touch.position.x < Screen.width / 2f)
                return;

            startPos = touch.position;
            isSwiping = true;
        }

        if ((touch.phase == TouchPhase.Ended ||
             touch.phase == TouchPhase.Canceled) && isSwiping)
        {
            CheckSwipe(touch.position);
            isSwiping = false;
        }
    }

    void CheckSwipe(Vector2 endPos)
    {
        Vector2 delta = endPos - startPos;

        if (Mathf.Abs(delta.x) < swipeThreshold)
            return;

        if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y))
            return;

        if (delta.x > 0)
            cameraController.RotateRight();
        else
            cameraController.RotateLeft();
    }
}