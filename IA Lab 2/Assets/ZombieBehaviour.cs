using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    public GameObject target;

    NavMeshAgent _agent;
    Animator _animator;

    [Header("PURSUE")]
    public float distanceToPursue = 10;
    public float lookAngleToPursue = 30;
    public LayerMask layerToPursue = 30; 
    public float stopPursueTime = 5.0f;

    [Header("WANDER")]
    public float updatePointTime = 10.0f;
    public Vector3 mapBounds;

    float cooldownWander = 0;
    float cooldownPursue = 0;
    Vector3 wanderTarget = Vector3.zero;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
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
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - transform.position;
        float lookAngle = Vector3.Angle(this.transform.forward, rayToTarget);
        if (lookAngle < lookAngleToPursue && rayToTarget.magnitude < distanceToPursue)
        {
            Vector3 shotRayPosition = transform.position;
            shotRayPosition.y += 1;
            if (Physics.Raycast(shotRayPosition, rayToTarget, out raycastInfo))
            {
                if (raycastInfo.collider.gameObject.tag == "Player")
                {
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

        Debug.Log(cooldownPursue);
        
        if (CanSeeTarget() || cooldownPursue < stopPursueTime)
        {
            Pursue();
        }
        else if (false)////SMELL
        {

        }
        else
        {
            if (cooldownWander > updatePointTime)
            {
                Wander();
                cooldownWander = 0;
            }
        }

        _animator.SetFloat("speed", _agent.velocity.magnitude);
    }
}

