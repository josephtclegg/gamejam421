using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnodController : MonoBehaviour
{
    public Transform agent;

    void FixedUpdate()
    {
        transform.rotation = agent.rotation * Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(agent.position.x, agent.position.y + 3, agent.position.z);
    }
}
