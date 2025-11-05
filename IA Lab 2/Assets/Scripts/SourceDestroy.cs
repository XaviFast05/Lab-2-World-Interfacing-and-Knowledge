using Unity.VisualScripting;
using UnityEngine;

public class SourceDestroy : MonoBehaviour
{
    public bool triggered;
    void Start()
    {
        Destroy(gameObject, 15f);
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.CompareTag("Smell Sensor"))
            {
                triggered = true;
            }
            if (other.CompareTag("Zombie"))
            {
                Destroy(gameObject);
            }
        }
    }
}
