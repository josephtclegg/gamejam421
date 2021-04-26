using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public GameController gc;
    public float MovementSpeed = 1;
    public float Gravity = 9.8f;
    public GameObject gnod;
    private float velocity = 0;
    private Camera cam;
    private bool isInFrontOfBathroom = false;
	public bool got = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        // player movement - forward, backward, left, right
        float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
        float vertical = Input.GetAxis("Vertical") * MovementSpeed;
        characterController.Move((cam.transform.right * horizontal + cam.transform.forward * vertical) * Time.deltaTime);

        // Gravity
        if (characterController.isGrounded)
        {
            velocity = 0;
        }
        else
        {
            velocity -= Gravity * Time.deltaTime;
            characterController.Move(new Vector3(0, velocity, 0));
        }

        RaycastHit hit;
        int layerMask = (1 << 9);
        if (Input.GetButtonDown("Fire1") && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10.0f, layerMask)) {
            if (hit.collider.transform.tag == "Door") {
                hit.collider.transform.gameObject.GetComponent<Door>().OpenDoor();
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N)) {
            gc.updateGameState();
        }
    }

    public CharacterController GetCharacterController() {
        return characterController;
    }

    public void SetIsInFrontOfBathroom(bool front) {
        isInFrontOfBathroom = front;
    }

    public bool IsInFrontOfBathroom() {
        return isInFrontOfBathroom;
    }
}
