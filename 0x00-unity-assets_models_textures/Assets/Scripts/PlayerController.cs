using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public CharacterController control;
	public Material body;
	public Material eyes;
	private Rigidbody rb;
	public Transform cam;
	public float speed;
	// Speed and Duration of player rotation
	private float turnSmoothTime = 10.0f;
	private float turnSmoothVelocity;
	// Jump-related vars
	private float jumpHeight = 1.0f;
	private float gravityValue = -9.81f;

	// Player input as X/Y axes
	private Vector2 _movementInput;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		setRandomColor(body);
		setRandomColor(eyes);
	}

	void OnMove(InputValue moveInput)
	{
		_movementInput = moveInput.Get<Vector2>();
	}

	void OnReset()
	{
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
	}

	void OnJump()
	{
		Debug.Log("Jump event activated");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		moveKbd();
		// if (Input.GetButtonDown("Jump"))
		// 	transform.Translate(new Vector3(0, 5.25f, 0));
 	}

	void moveKbd()
	{
		Vector3 movement = Quaternion.Euler(0, cam.eulerAngles.y, 0) * new Vector3(_movementInput.x, 0.0f, _movementInput.y).normalized;

		// Dead code. So is flooring the Z and X values, commented out below.
		// Now, we capture the intended camera angle by directly referencing the Y value of
		// The camera's rotation and multiplying by the input vector. Much cleaner.

		// movement = cam.forward * movement.z + cam.right * movement.x;
		// expectedRotation.z = expectedRotation.x = 0;

		// This one orients the player according to the camera, instead we orient
		// towards the player's movement. Here for posterity.
		// transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y);

		if (movement.magnitude != 0)
		{
			Quaternion expectedRotation = Quaternion.LookRotation(movement, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, expectedRotation, turnSmoothTime * Time.deltaTime);

			// Alternatively, the math can be done manually with this approach. I think above is much cleaner.
			// 	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			// 	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			// 	transform.rotation = (Quaternion.Euler(0f, angle, 0f));
		}
		control.Move(movement * Time.deltaTime * speed);
		control.Move(Physics.gravity * Time.deltaTime);

	}

	void setRandomColor(Material update)
	{
		if (update is Material Update)
			Update.SetColor("_Color", new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255)));

	}
}
