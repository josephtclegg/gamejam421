using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
	public GameObject goContainer;
	public GameObject player;
    private bool got = false;

    // Start is called before the first frame update
    void Start()
    {
        setVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

	void toggleVisibility()
	{
		setVisibility(!goContainer.activeSelf);
	}

	void setVisibility(bool visible)
	{
		if (visible)
			Cursor.lockState = CursorLockMode.None;
		else
			Cursor.lockState = CursorLockMode.Locked;
		goContainer.SetActive(visible);
	}
}
