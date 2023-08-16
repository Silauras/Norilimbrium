using UnityEngine;

public class CameraMovementSystem : MonoBehaviour
{
    public Transform target; // Ссылка на персонажа
    public float smoothSpeed = 0.125f;
    public float smoothFactor = 2f; // Множитель для экспоненциальной интерполяции
    public Vector2 offset;
    public Vector2 cursorOffsetFactor = new Vector2(0.1f, 0.1f);
    public Vector2 cameraBounds = new Vector2(5f, 5f);

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z);

        Vector3 cursorPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorOffset = new Vector2(
            cursorPosition.x - target.position.x,
            cursorPosition.y - target.position.y
        ) * cursorOffsetFactor;

        desiredPosition += new Vector3(cursorOffset.x, cursorOffset.y, 0);

        float clampedX = Mathf.Clamp(desiredPosition.x, target.position.x - cameraBounds.x, target.position.x + cameraBounds.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, target.position.y - cameraBounds.y, target.position.y + cameraBounds.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, transform.position.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, 1 - Mathf.Exp(-smoothFactor * Time.deltaTime));
        transform.position = smoothedPosition;
    }
}