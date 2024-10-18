using UnityEngine;
using UnityEngine.UI;

public class NpcInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 3f;
    public LayerMask npcLayer;
    public GameObject interactIndicator;
    public float checkInterval = 0.5f; // Интервал проверки в секундах

    private void Start()
    {
        InvokeRepeating(nameof(CheckForNpc), 0f, checkInterval);
    }

    private void CheckForNpc()
    {
        var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        interactIndicator.gameObject.SetActive(Physics.Raycast(ray, out var hit, interactionRange, npcLayer));
    }
}
