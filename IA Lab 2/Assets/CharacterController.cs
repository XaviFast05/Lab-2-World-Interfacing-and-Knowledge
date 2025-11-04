using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Camera _camera;
    private Animator _animator;

    public GameObject destinationIndicator;
    public LayerMask groundLayer;
    public float walkSpeed;
    public float runSpeed;

    private bool run;
    private float runTimer;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;

    }

    private void Start()
    {
        run = false;
        runTimer = 0.5f;
    }

    void Update()
    {
        runTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (runTimer < 0.5f)
            {
                run = true;
            }
            else run = false;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            bool didHit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayer);


            if (didHit)
            {
                Vector3 targetPoint = hitInfo.point;

                NavMeshHit navHit;

                bool foundValidPoint = NavMesh.SamplePosition(
                    targetPoint,
                    out navHit,
                    4.0f,
                    NavMesh.AllAreas
                );

                if (foundValidPoint)
                {
                    _agent.SetDestination(navHit.position);
                    runTimer = 0;
                }  
            }
        }

        if (run) _agent.speed = runSpeed;
        else _agent.speed = walkSpeed;

        if (_agent.velocity.magnitude > 0.5f)
        {
            destinationIndicator.SetActive(true);
            destinationIndicator.transform.position = _agent.destination;
        }
        else destinationIndicator.SetActive(false);

            _animator.SetFloat("speed", _agent.velocity.magnitude);
    }
}
