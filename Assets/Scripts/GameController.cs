using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private string keyfile = "/home/eggert/private/cs35lanswers_1337.key";
	private string target = "http://joecl.org/";

	public GameObject player;
	public GameObject gnod;
	public GameObject goContainer;
    private GameStateMachine gsm;

    // Start is called before the first frame update
    void Start()
    {
		gsm = new GameStateMachine();
    }

    public void updateGameState()
    {
        Debug.Log(string.Format("Old state: {0}", gsm.GetCurrentState()));
        gsm.MakeTransition(Transition.CompletedGoal);
        Debug.Log(string.Format("New state: {0}", gsm.GetCurrentState()));
    }

		public State GetCurrentState() {
			return gsm.GetCurrentState();
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

		public string EscapeKeyFile() {
			return keyfile;
		}

		public string EscapeTarget() {
			return target;
		}
}
