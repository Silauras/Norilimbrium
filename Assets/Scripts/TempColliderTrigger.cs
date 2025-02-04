using UnityEngine;
using UnityEngine.Events;

public class TempColliderTrigger : MonoBehaviour
{
    public UnityEvent MethodToCall;

    private bool playerInRange;

    void Start()
    {
        playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Player Clicked");
            MethodToCall?.Invoke();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Player Entered the Trigger");
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Player Exited the Trigger");
            playerInRange = false;
        }
    }
}
