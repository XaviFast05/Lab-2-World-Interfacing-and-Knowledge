using Unity.VisualScripting;
using UnityEngine;

public class SourceDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Zombie"))
        {
            Destroy(gameObject);
        }
    }
}
