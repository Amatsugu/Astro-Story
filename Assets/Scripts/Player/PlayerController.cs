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
	public AudioSource pickupSoundSource;

	[Header("Input")]
	public InputActionReference steerAction;
	public InputActionReference thrustAction;

	private float _curSpeed;
	private float _curTurnSpeed;
	private float _steer;
	private float _thrust;

	private Transform _transform;
	private AudioSource _audioSource;

	// Start is called before the first frame update
	private void Start()
	{
		_transform = transform;
		_audioSource = GetComponent<AudioSource>();

		steerAction.action.started += Steer;
		steerAction.action.canceled += Steer;
		steerAction.action.performed += Steer;
		thrustAction.action.started += Thrust;
		thrustAction.action.canceled += Thrust;
		thrustAction.action.performed += Thrust;
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

	public void PlayPickupSound(AudioClip clip)
	{
		pickupSoundSource.PlayOneShot(clip);
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
			if(_curSpeed < 0)
				_curSpeed = 0;
		}

		if (_steer != 0)
			_curTurnSpeed += steeringSpeed * Time.deltaTime * _steer;
		else
		{
			if (_curTurnSpeed > 0)
				_curTurnSpeed -= turnDrag * Time.deltaTime;
			if (_curTurnSpeed < 0)
				_curTurnSpeed += turnDrag * Time.deltaTime;
			if(Mathf.Abs(_curTurnSpeed) < 0.1f)
				_curTurnSpeed = 0;
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