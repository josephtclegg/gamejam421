using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingDoor : MonoBehaviour
{
    public int doorNumber = 9999;
    public string doorText = "";
    public float chanceOfDisappearing = 33.0f;
    public bool isLocked = false;

    private GameObject door;
    private GameObject facade;
    private bool revealDoor = true;
    private bool isOpen = false;
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            if (child.tag == "Door")
                door = child.gameObject;
            if (child.tag == "Wall")
                facade = child.gameObject;
            if (child.tag == "DoorL3")
                door = child.gameObject;
        }

        door.SetActive(true);
        facade.SetActive(false);
        renderer = door.transform.GetChild(0).GetComponent<Renderer>();
        if (doorText.Length > 0)
            door.GetComponent<Door>().SetDoorText(doorText);
        else
            door.GetComponent<Door>().SetDoorNumber(doorNumber);
    }

    // Update is called once per frame
    void Update()
    {
        door.GetComponent<Door>().SetLocked(isLocked);
        if (renderer.isVisible && revealDoor) {
            revealDoor = false;
            if (Random.Range(0.0f, 100.0f) > chanceOfDisappearing)
                ShowDoor();
            else
                ShowWall();
        }

        if (!renderer.isVisible && !revealDoor)
            revealDoor = true;
    }

    public void SetLocked(bool l) {
        isLocked = l;
    }

    void ShowDoor()
    {
        renderer = door.transform.GetChild(0).GetComponent<Renderer>();
        door.SetActive(true);
        facade.SetActive(false);
    }

    void ShowWall()
    {
        renderer = facade.GetComponent<Renderer>();
        door.SetActive(false);
        facade.SetActive(true);
    }

    public void OpenDoor()
    {
        if (!revealDoor) return;

        door.GetComponent<Door>().OpenDoor();
    }
}
