using UnityEngine;

public class BathroomTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<PlayerController>() == null) return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        player.SetIsInFrontOfBathroom(true);
        Debug.Log("player in front of br");
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<PlayerController>() == null) return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        player.SetIsInFrontOfBathroom(false);
        Debug.Log("player not in front of br");
    }
}
