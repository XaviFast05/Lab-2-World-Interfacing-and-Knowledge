using UnityEngine;
using UnityEngine.Events;




public class SmellSensor : MonoBehaviour
{
    public UnityEvent<Transform> onSmellDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Smell Source") && !other.gameObject.GetComponent<SourceDestroy>().triggered)
        {
            onSmellDetected.Invoke(other.transform);
        }
    }
}
