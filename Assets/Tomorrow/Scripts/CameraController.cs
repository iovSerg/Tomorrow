using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Tomorrow
{

	public class CameraController : MonoBehaviour
	{
		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;

		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;

		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;

		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;

		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		[Tooltip("Sensivity rotate camera")]
		public float _sensivity = 5f;

		private Camera mainCamera;
		private PlayerInput playerInput;

		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		private void Start()
		{
			mainCamera = Camera.main;
			playerInput = GetComponent<PlayerInput>();
			_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
		}
		private void LateUpdate()
		{
			CameraRotation();
		}
		private void CameraRotation()
		{
		
	
			_cinemachineTargetYaw += playerInput.Look.x * _sensivity;
			_cinemachineTargetPitch += playerInput.Look.y * _sensivity;
			

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
				_cinemachineTargetYaw, 0.0f);
		}
		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}
	}
}
