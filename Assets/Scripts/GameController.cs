using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public GameObject player;
	public GameObject gnod;
	public GameObject goContainer;
	private GameStateMachine gsm = new GameStateMachine();

    // Start is called before the first frame update
    void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		switch (gsm.GetCurrentState())
		{
			case State.NoGoalsCompleted:
				//state = State.FewGoalsCompleted;
				break;
			case State.FewGoalsCompleted:
				//state = State.SomeGoalsCompleted;
				break;
			case State.SomeGoalsCompleted:
				//state = State.ManyGoalsCompleted;
				break;
			case State.ManyGoalsCompleted:
				//state = State.AllGoalsCompleted;
				break;
			case State.CaughtByGnod:
				goContainer.SetActive(true);
				if (Input.anyKey)
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				break;
			default:
				// Do nothing
				break;
		}

		if (Vector3.Distance(player.transform.position, gnod.transform.position) < 2.0)
		{
			gsm.MakeTransition(Transition.GotCaughtByGnod);
		}
	}
}
