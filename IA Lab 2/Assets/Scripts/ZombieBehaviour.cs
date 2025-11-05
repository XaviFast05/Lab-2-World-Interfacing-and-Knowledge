using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    public GameObject target;

    NavMeshAgent _agent;
    Animator _animator;
    ZombiesManager _zombiesManager;

    [Header("PURSUE")]
    public float distanceToPursue = 20;
    public float lookAngleToPursue = 30;
    public float stopPursueTime = 5.0f;
    public float runSpeed;

    [Header("WANDER")]
    public float updatePointTime = 10.0f;
    public Vector3 mapBounds;
    public float walkSpeed;

    float cooldownWander = 0;
    public float cooldownPursue = 0;
    Vector3 wanderTarget = Vector3.zero;
    Transform smellTarget = null;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _zombiesManager = GetComponentInParent<ZombiesManager>();
    }

    void Start()
    {
        cooldownWander = updatePointTime;
        cooldownPursue = stopPursueTime;
    }

    void Seek(Vector3 location)
    {
        _agent.SetDestination(location);
    }

    void Pursue()
    {
        Vector3 targetPosition = target.transform.position;

        Seek(targetPosition);
    }

    void Wander()
    {
        wanderTarget = new Vector3(UnityEngine.Random.Range(mapBounds.x / 2, -mapBounds.x / 2),
                                        0,
                                        UnityEngine.Random.Range(mapBounds.z / 2, -mapBounds.z / 2));
        Seek(wanderTarget);
    }

    bool CanSeeTarget()
    {
        if (target == null)
        {
            return false;
        }

        Vector3 origin = transform.position;
        origin.y += 1.0f;

        Vector3 targetCenter = target.transform.position;
        targetCenter.y += 1.0f; 

        Vector3 rayDirection = targetCenter - origin;

        float distanceToTarget = rayDirection.magnitude;

        float lookAngle = Vector3.Angle(transform.forward, rayDirection);

        if (lookAngle < lookAngleToPursue && distanceToTarget < distanceToPursue)
        {
            RaycastHit hit;

            bool didHit = Physics.Raycast(origin, rayDirection.normalized, out hit, distanceToTarget);
           
            Debug.DrawRay(origin, rayDirection);

            if (didHit)
            {
                if (hit.transform.tag == "Player")
                {
                    _zombiesManager.OnSeePlayer(target.transform.position);
                    cooldownPursue = 0;
                    return true;
                }
            }
        }

        return false;
    }

    void Update()
    {
        cooldownWander += Time.deltaTime;
        cooldownPursue += Time.deltaTime;
        
        if (CanSeeTarget() || cooldownPursue < stopPursueTime)
        {
            _agent.speed = runSpeed;
            Pursue();
        }
        else if (smellTarget != null)
        {
            _agent.speed = walkSpeed;
            Seek(smellTarget.position);
        }
        else
        {
            _agent.speed = walkSpeed;
            if (cooldownWander > updatePointTime)
            {
                Wander();
                cooldownWander = 0;
            }
        }

        _animator.SetFloat("speed", _agent.velocity.magnitude);
    }

    public void SetSmellTransform(Transform T)
    {
        _zombiesManager.OnSeeBlood(T);
        smellTarget = T;
    }

    public void SeeSmell(Transform T)
    {
        smellTarget = T;
    }

    public void SeePlayer(Vector3 position)
    {
        cooldownPursue = 0;
        Seek(position);
    }
}

