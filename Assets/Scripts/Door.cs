using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private TextMesh doorNumText;
    private bool isOpen = false;
    private string doorNumber = "9999";

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

    public string GetDoorNumber()
    {
        return doorNumber;
    }

    public void SetDoorNumber(int num)
    {
        doorNumber = num.ToString();
    }

    public void OpenDoor()
    {
        if (!isOpen)
            transform.RotateAround(transform.parent.position, Vector3.up, -90);
        else
            transform.RotateAround(transform.parent.position, Vector3.up, 90);

        isOpen = !isOpen;
    }
}
