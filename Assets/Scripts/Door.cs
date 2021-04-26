using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private TextMesh doorNumText;
    private bool isOpen = false;
    private string doorNumber = "9999";
    private bool isLocked;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Door Number")
                doorNumText = child.gameObject.GetComponent<TextMesh>();
        }
    }

    void Update()
    {
        doorNumText.text = doorNumber;
    }

    public void SetLocked(bool locked) {
        isLocked = locked;
    }

    public string GetDoorNumber()
    {
        return doorNumber;
    }

    public void SetDoorNumber(int num)
    {
        doorNumber = num.ToString();
    }

    public void SetDoorText(string text)
    {
        doorNumber = text;
    }

    public void OpenDoor()
    {
        if (isLocked) return;

        if (!isOpen)
            transform.RotateAround(transform.parent.position, Vector3.up, -90);
        else
            transform.RotateAround(transform.parent.position, Vector3.up, 90);

        isOpen = !isOpen;
    }
}
