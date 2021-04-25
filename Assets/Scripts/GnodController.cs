using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnodController : MonoBehaviour
{
    public GnodNavAgent agent;
    public float maxPitch = 1.4f;
    public float maxDist = 20f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        transform.rotation = agent.transform.rotation * Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y + 3, agent.transform.position.z);

        float distance = (agent.follow.position - transform.position).magnitude;
        if (distance < maxDist) {
            float distPercent = 1.0f - (distance/maxDist);
            audioSource.pitch = 1.0f + (distPercent * (maxPitch - 1.0f));
        } else {
            audioSource.pitch = 1.0f;
        }
    }
}
