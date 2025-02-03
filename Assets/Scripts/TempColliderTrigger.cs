using UnityEngine;
using UnityEngine.Events;

public class TempColliderTrigger : MonoBehaviour
{
    public UnityEvent MethodToCall;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object Entered the Trigger");
        MethodToCall?.Invoke();
    }
}
