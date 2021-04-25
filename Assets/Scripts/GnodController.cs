using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnodController : MonoBehaviour
{
    public Transform follow;
    public float moveSpeed = 1f;
    public float rotationSpeed = 3f;

    private Vector3 direction;
    private Quaternion lookRot;

    void Update()
    {
        direction = (follow.position - transform.position).normalized;
        lookRot = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, follow.position, Time.deltaTime * moveSpeed);
    }
}
