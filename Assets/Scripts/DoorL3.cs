using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorL3 : MonoBehaviour
{
    public GameObject controller;
    private TextMesh doorNumText;
    private bool isOpen = false;
    private string doorNumber = "5144";

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Door Number")
                doorNumText = child.gameObject.GetComponent<TextMesh>();
        }
    }

    public State getGameState()
    {
        return controller.GetComponent<GameController>().getGameState();
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

    public void updateGameState()
    {
        controller.GetComponent<GameController>().updateGameState();
    }

    public void OpenDoor()
    {
        if(getGameState() == State.NoGoalsCompleted || getGameState() == State.FewGoalsCompleted)
        {
            return;
        }
        if(getGameState() == State.SomeGoalsCompleted)
        {
            updateGameState();
        }
        if (!isOpen)
            transform.RotateAround(transform.parent.position, Vector3.up, -90);
        else
            transform.RotateAround(transform.parent.position, Vector3.up, 90);

        isOpen = !isOpen;
    }
}

