using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 targetOffset;
    [SerializeField] Vector3 cameraRotation;
    private Quaternion cameraQuatRotation;

    private void Awake()
    {
        cameraQuatRotation = Quaternion.Euler(cameraRotation);
    }

    void Update()
    {
        transform.position = target.position + targetOffset;
        transform.rotation = cameraQuatRotation;
    }
}
