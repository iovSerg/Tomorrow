using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

namespace Tomorrow
{
	public class MovementController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;

		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;

		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		public AudioClip LandingAudioClip;
		public AudioClip[] FootstepAudioClips;
		[Range(0, 1)] public float FootstepAudioVolume = 0.5f;

		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;

		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;

		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		private CharacterController controller;
		private CameraController cameraController;
		private PlayerInput playerInput;
		private Camera mainCamera;

		public float _speed;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private Vector3 _velocity;
		private float _gravity = -9.81f;
		private float _gravityMultiply = 3f;

		private void Start()
		{
			controller = GetComponent<CharacterController>();
			playerInput = GetComponent<PlayerInput>();
			mainCamera = Camera.main;
		}
		private void Update()
		{
			ApllyGravity();
			Move();

		}
		public float targetSpeed;
		public float currentHorizontalSpeed;
		private void Move()
		{
		    targetSpeed = playerInput.Sprint ? MoveSpeed : SprintSpeed;
			if (playerInput.Move == Vector2.zero) targetSpeed = 0.0f;
			currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f,controller.velocity.z).magnitude;

			float speedOffset = 0.1f;

			if (currentHorizontalSpeed < targetSpeed - speedOffset ||
				currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * playerInput.Magnituda,
					Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}


			if (playerInput.Move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(playerInput.Move.x, playerInput.Move.y) * Mathf.Rad2Deg +
								  mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
					RotationSmoothTime);

				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}
			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			controller.Move((targetDirection * _speed * Time.deltaTime) + _velocity * Time.deltaTime);
		}
		private void ApllyGravity()
		{
			if (controller.isGrounded && _velocity.y < 0.0f)
			{
				_velocity.y = -1.0f;
			}
			else
			{
				_velocity.y += _gravity * _gravityMultiply * Time.deltaTime;
			}

		}
	}
}
