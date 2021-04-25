using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnodNavAgent : MonoBehaviour
{
    public Transform follow;
    public Transform[] patrolPoints;
    public float moveSpeed = 1f;
    public float rotationSpeed = 3f;

    private Vector3 direction;
    private Quaternion lookRot;
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        GoToNextPoint();
    }

    void FixedUpdate()
    {
        direction = (follow.position - transform.position).normalized;

        RaycastHit hit;
        int layerMask = (1 << 8) | (1 << 9);

        if(Physics.Raycast(transform.position + Vector3.up, direction, out hit, Mathf.Infinity, layerMask)) {
            Debug.DrawRay(transform.position + Vector3.up, direction * hit.distance, Color.red);
            if (hit.collider.transform.tag == "Player") {
                agent.isStopped = true;
                lookRot = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
                transform.position = Vector3.MoveTowards(transform.position, follow.position, Time.deltaTime * moveSpeed);
            } else {
                agent.isStopped = false;
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GoToNextPoint();
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        foreach (Transform point in patrolPoints)
            Gizmos.DrawSphere(point.position, 0.2f);
    }

    void GoToNextPoint() {
        if (patrolPoints.Length <= 0) return;

        agent.destination = patrolPoints[destPoint].position;

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }
}
