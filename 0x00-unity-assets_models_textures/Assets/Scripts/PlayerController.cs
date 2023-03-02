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
	private float _maxJumpHeight;
	private float heightJumped;
	private bool jumped = false;
	private bool disallownewjump = false;
	private Vector3 movement = Vector3.zero;

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
		// SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
		setRandomColor(body);
		setRandomColor(eyes);
	}

	void OnJump()
	{
		if (!disallownewjump || control.isGrounded)
		{
			jumped = true;
			heightJumped = 0;
		}
		if (!control.isGrounded)
		{
			disallownewjump = true;
			_maxJumpHeight = 15f;
		}
		else
		{
			disallownewjump = false;
			_maxJumpHeight = 30f;
		}

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		captureAndSync();
		handleJump();
		control.Move(movement * Time.deltaTime);
		resetPosition();
 	}

	void captureAndSync()
	{
		movement = (Quaternion.Euler(0, cam.eulerAngles.y, 0) * new Vector3(_movementInput.x, 0.0f, _movementInput.y).normalized) * speed;

		if (movement.magnitude != 0)
			syncAvatarWithDirection();

	}

	void syncAvatarWithDirection()
	{
		Quaternion expectedRotation = Quaternion.LookRotation(movement, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, expectedRotation, turnSmoothTime * Time.deltaTime);

		// Alternatively, the math can be done manually with this approach. I think above is much cleaner.
		// 	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		// 	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
		// 	transform.rotation = (Quaternion.Euler(0f, angle, 0f));
	}

	void handleJump()
	{
		if (jumped && heightJumped < _maxJumpHeight)
		{
			movement.y = (heightJumped += (_maxJumpHeight / 15));
			transform.localScale += new Vector3(0,
							    disallownewjump ? .0166f : .0333f,
							    disallownewjump ? -.010f : -.020f);
		}
		else
		{
			restoreShape();
			jumped = false;
			movement.y = transform.position.y > 15f
				? Physics.gravity.y * 5
				: Physics.gravity.y;
		}
	}

	void restoreShape(float xDesired = 1f, float yDesired = 1f, float zDesired = 1f)
	{
		if (transform.localScale.y > yDesired)
			transform.localScale -= new Vector3(0, .0333f, 0);
		if (transform.localScale.z < zDesired)
			transform.localScale += new Vector3(0, 0, .02f);
	}

	void resetPosition()
	{

		if (transform.position.y < -15f)
		{
			control.enabled = false;
			transform.position = new Vector3(0, 100, 0);
			control.enabled = true;
		}
	}

	void setRandomColor(Material update)
	{
		if (update is Material Update)
			Update.SetColor("_Color", new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255)));

	}
}
