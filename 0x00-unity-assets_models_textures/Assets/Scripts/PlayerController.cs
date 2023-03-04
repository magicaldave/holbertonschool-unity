using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public CharacterController control;
	public Material body, eyes, nose, hat;
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
	[SerializeField] private float zScaleFactor, yScaleFactor;

	// Player input as X/Y axes
	private Vector2 _movementInput;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		setRandomColors();
	}

	void OnMove(InputValue moveInput)
	{
		_movementInput = moveInput.Get<Vector2>();
	}

	void OnReset()
	{
		// SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
		setRandomColors();
	}

	void OnJump()
	{
		if (!disallownewjump || control.isGrounded)
		{

			if (!control.isGrounded)
			{
				disallownewjump = true;
				_maxJumpHeight = 20f;
			}
			else
			{
				disallownewjump = false;
				_maxJumpHeight = 30f;
			}
			jumped = true;
			heightJumped = 0;
		}

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		captureAndSync();
		handleJump();
		 RaycastHit hit;
		 // int layerMask = 1 << 8;
		 // Does the ray intersect any objects excluding the player layer
		 if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.5f))
		 {
			 if(hit.collider.tag == "MobilePlatform")
			 {
				 // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.yellow);
				 // Debug.Log(movement);
				 movement.x += (hit.collider.transform.position.x - transform.position.x);
				 // Debug.Log(movement);
				 // Debug.Log("Did Hit");
			 }
		 }
		 else
		 {
			 Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.white);
			 // Debug.Log("Did not Hit");
		 }
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
			movement.y += ((heightJumped += (_maxJumpHeight / 20)) * 1.5f);
			// Quite literally squashes and stretches the character.
			// restoreShape brings them back to their original form.
			transform.localScale += new Vector3(0, yScaleFactor, zScaleFactor);
		}
		else
		{
			jumped = false;
			if (transform.localScale != Vector3.one)
				transform.localScale -= new Vector3(0, yScaleFactor, zScaleFactor);
        }

		movement.y += Physics.gravity.y;
	}

	void resetPosition()
	{

		if (transform.position.y < -15f)
		{
			control.enabled = false;
			transform.position = new Vector3(0, 25, 0);
			control.enabled = true;
		}
	}

	void setRandomColors()
	{
		body.SetColor("_Color", new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255)));
		eyes.SetColor("_Color", new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255)));
		nose.SetColor("_Color", new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255)));
		hat.SetColor("_Color", new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255)));
	}
}
