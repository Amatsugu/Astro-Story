using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Properties")]
	public float speed = 10;

	public float acceleration = 1;
	public float boostSpeed = 20;
	public float drag = 2;

	public float turnSpeed = 1;
	public float turnDrag = 1;

	[Header("Sounds")]
	public float minPitch = 1;
	public float maxPitch = 1.2f;
	public float boostPitch = 1.5f;

	private float _curSpeed;
	private float _curTurnSpeed;
	private Transform _transform;
	private AudioSource _audioSource;

	// Start is called before the first frame update
	private void Start()
	{
		_transform = transform;
		_audioSource = GetComponent<AudioSource>();
		_audioSource.loop = true;
		if (!_audioSource.isPlaying)
			_audioSource.Play();
	}

	// Update is called once per frame
	private void Update()
	{
		var isBoosting = Input.GetKey(KeyCode.Space);
		var curMaxSpeed = isBoosting ? speed : boostSpeed;
		if (Input.GetKey(KeyCode.W))
		{
			_curSpeed += acceleration * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			_curSpeed -= acceleration * Time.deltaTime;
		}
		else
		{
			if (_curSpeed != 0)
			{
				_curSpeed -= drag * Time.deltaTime;
			}
		}

		if (Input.GetKey(KeyCode.A))
		{
			if(_curTurnSpeed > 0)
				_curTurnSpeed = 0;
			_curTurnSpeed -= turnSpeed * Time.deltaTime;
		}else if (Input.GetKey(KeyCode.D))
		{
			if (_curTurnSpeed < 0)
				_curTurnSpeed = 0;
			_curTurnSpeed += turnSpeed * Time.deltaTime;
		}
		else
		{
			if(_curTurnSpeed > 0)
				_curTurnSpeed -= turnDrag * Time.deltaTime;
			if (_curTurnSpeed < 0)
				_curTurnSpeed += turnDrag * Time.deltaTime;
		}




		_curSpeed = Mathf.Clamp(_curSpeed, 0, curMaxSpeed);

		var angle = _transform.rotation.eulerAngles.y;

		var rot = Quaternion.AngleAxis(angle + _curTurnSpeed * Time.deltaTime, Vector3.up);
		_transform.rotation = rot;


		var fwd = _transform.forward;
		var move = _curSpeed * Time.deltaTime * fwd;

		Debug.DrawRay(_transform.position, fwd, Color.yellow);
		Debug.DrawRay(_transform.position, move, Color.magenta);

		_transform.Translate(move, Space.World);

		var pitch = minPitch;
		var speedScale = _curSpeed / curMaxSpeed;
		if (isBoosting)
			pitch = Mathf.Lerp(minPitch, boostPitch, speedScale);
		else
			pitch = Mathf.Lerp(minPitch, maxPitch, speedScale);
		_audioSource.pitch = pitch;
	}
}