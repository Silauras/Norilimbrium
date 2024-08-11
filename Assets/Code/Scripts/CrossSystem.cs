using UnityEngine;

public class CrossSystem : MonoBehaviour
{
    public Texture2D cursorTexture; // Путь к текстуре прицела
    public Vector2 cursorHotspot = Vector2.zero; // Позиция "горячей точки" для курсора

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
}
