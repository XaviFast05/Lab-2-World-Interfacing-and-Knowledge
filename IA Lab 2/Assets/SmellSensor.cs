using UnityEngine;
using UnityEngine.Events;




public class SmellSensor : MonoBehaviour
{
    public UnityEvent<Transform> onSmellDetected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Smell Source"))
        {
            onSmellDetected?.Invoke(other.transform);
        }

    }

}
