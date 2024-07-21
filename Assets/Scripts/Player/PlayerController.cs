using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Player Properties")]
	public float speed = 10;

	public float acceleration = 1;
	public float drag = 2;

	public float steeringSpeed = 1;
	public float maxTurnSpeed = 1;
	public float turnDrag = 1;

	[Header("Sounds")]
	public float minPitch = 1;

	public float maxPitch = 1.2f;

	private float _curSpeed;
	private float _curTurnSpeed;
	private float _steer;
	private float _thrust;

	private Transform _transform;
	private AudioSource _audioSource;
	private PlayerInput _playerInput;
	private InputAction _steerAction;
	private InputAction _thrustAction;

	// Start is called before the first frame update
	private void Start()
	{
		_transform = transform;
		_audioSource = GetComponent<AudioSource>();
		_playerInput = GetComponent<PlayerInput>();
		_steerAction = _playerInput.actions.FindAction("Steer");
		_thrustAction = _playerInput.actions.FindAction("Thrust");

		_steerAction.started += Steer;
		_steerAction.canceled += Steer;
		_steerAction.performed += Steer;
		_thrustAction.started += Thrust;
		_thrustAction.canceled += Thrust;
		_thrustAction.performed += Thrust;
		_audioSource.loop = true;
		if (!_audioSource.isPlaying)
			_audioSource.Play();
	}

	

	private void Steer(InputAction.CallbackContext context)
	{
		_steer = context.ReadValue<float>();
	}

	private void Thrust(InputAction.CallbackContext context)
	{
		_thrust = context.ReadValue<float>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (GameManager.IsInDialouge)
			return;
		var curMaxSpeed = speed;

		if (_thrust != 0)
		{
			_curSpeed += acceleration * Time.deltaTime * _thrust;
		}
		else
		{
			if (_curSpeed != 0)
			{
				_curSpeed -= drag * Time.deltaTime;
			}
		}

		if (_steer != 0)
			_curTurnSpeed += steeringSpeed * Time.deltaTime * _steer;
		else
		{
			if (_curTurnSpeed > 0)
				_curTurnSpeed -= turnDrag * Time.deltaTime;
			if (_curTurnSpeed < 0)
				_curTurnSpeed += turnDrag * Time.deltaTime;
		}

		if (_curTurnSpeed > maxTurnSpeed)
			_curTurnSpeed = maxTurnSpeed;
		if (_curTurnSpeed < -maxTurnSpeed)
			_curTurnSpeed = -maxTurnSpeed;

		_curSpeed = Mathf.Clamp(_curSpeed, 0, curMaxSpeed);

		var angle = _transform.rotation.eulerAngles.y;

		var rot = Quaternion.AngleAxis(angle + _curTurnSpeed * Time.deltaTime, Vector3.up);
		_transform.rotation = rot;

		var fwd = _transform.forward;
		var move = _curSpeed * Time.deltaTime * fwd;

		Debug.DrawRay(_transform.position, fwd, Color.yellow);
		Debug.DrawRay(_transform.position, move, Color.magenta);

		_transform.Translate(move, Space.World);

		var speedScale = _curSpeed / curMaxSpeed;
		var pitch = Mathf.Lerp(minPitch, maxPitch, speedScale);
		_audioSource.pitch = pitch;
	}
}