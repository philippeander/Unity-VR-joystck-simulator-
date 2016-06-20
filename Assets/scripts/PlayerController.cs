using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	private float thrusterForce = 1000f;

	[SerializeField]
	private float thrusterFuelBurnSpeed = 1f;
	[SerializeField]
	private float thrusterFuelRegenSpeed = 0.3f;
	private float thrusterFuelAmount = 1f;

	public float GetThrusterFuelAmount ()
	{
		return thrusterFuelAmount;
	}

	[SerializeField]
	private LayerMask environmentMask;

	[Header("Spring settings:")]
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

	// Component caching
	private PlayerMotor motor;
	private ConfigurableJoint joint;


	private float _xMov;
	private float _zMov;
	private float _yRot;
	private float _xRot;

	private PhotonView photonView;

	public GameObject gameManager;
	private GameManager s_gameManager;

	void Awake(){
		gameManager = GameObject.Find("GameManager");
		s_gameManager = gameManager.GetComponent<GameManager> ();
	}

	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();

		SetJointSettings(jointSpring);
	}



	void FixedUpdate ()
	{
		_xMov = s_gameManager._xMov;
		_zMov = s_gameManager._zMov;
		_yRot = s_gameManager._yRot;
		_xRot = s_gameManager._xRot;

		//photonView.RPC ("MyGetInputs", PhotonTargets.All );
		//Setting target position for spring
		//This makes the physics act right when it comes to
		//applying gravity when flying over objects
		RaycastHit _hit;
		if (Physics.Raycast (transform.position, Vector3.down, out _hit, 100f, environmentMask))
		{
			joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
		} else
		{
			joint.targetPosition = new Vector3(0f, 0f, 0f);
		}

		//float _xMov = CrossPlatformInputManager.GetAxis("Horizontal");
		//float _zMov = CrossPlatformInputManager.GetAxis("Vertical");


		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		// Animate movement

		//Apply movement
		motor.Move(_velocity);

		//Calculate rotation as a 3D vector (turning around)
		//float _yRot = CrossPlatformInputManager.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (turning around)
		//float _xRot = CrossPlatformInputManager.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		motor.RotateCamera(_cameraRotationX);

		// Calculate the thrusterforce based on player input
		Vector3 _thrusterForce = Vector3.zero;
		if (Input.GetButton ("Jump") && thrusterFuelAmount > 0f)
		{
			thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

			if (thrusterFuelAmount >= 0.01f)
			{
				_thrusterForce = Vector3.up * thrusterForce;
				SetJointSettings(0f);
			}
		} else
		{
			thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
			SetJointSettings(jointSpring);
		}

		thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

		// Apply the thruster force
		motor.ApplyThruster(_thrusterForce);

	}

//	[PunRPC]
//	public void MyGetInputs(){
//		//Calculate movement velocity as a 3D vector
//		_xMov = CrossPlatformInputManager.GetAxis("Horizontal");
//		_zMov = CrossPlatformInputManager.GetAxis("Vertical");
//
//		_yRot = CrossPlatformInputManager.GetAxisRaw("Mouse X");
//		_xRot = CrossPlatformInputManager.GetAxisRaw("Mouse Y");
//	}

	private void SetJointSettings (float _jointSpring)
	{
		joint.yDrive = new JointDrive {
			positionSpring = _jointSpring,
			maximumForce = jointMaxForce
		};
	}

}
