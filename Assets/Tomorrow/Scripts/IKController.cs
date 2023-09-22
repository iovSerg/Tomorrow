using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomorrow
{
	public class IKController : MonoBehaviour
	{
		private Animator animator;
		private WeaponController weaponController;

		[Header("IK Weapon Hand")]
		[SerializeField] private Transform leftHand;
		[SerializeField] private Transform leftHint;

		[SerializeField] private Transform rightHandTarget;
		[SerializeField] private Transform leftHandTarget;

		[SerializeField] private Transform rightHand;
		[SerializeField] private Transform rightHint;

		[SerializeField] private Transform spine1;
		[SerializeField] private Transform lookAtPosition;

		[Header("IK Weapon Holder")]
		[SerializeField] private Transform rightHolder;
		[SerializeField] private Transform leftHolder;

		[Space(2)]
		[Header("Weight IK")]
		[SerializeField] private float _leftHandWeight = 0f;
		[SerializeField] private float _rightHandWeight = 0f;

		[SerializeField] private float _lookWeight;
		[SerializeField] private float _bodyWeight;
		[SerializeField] private float _headWeight;
		[SerializeField] private float _eyesWeight;
		[SerializeField] private float _clampWeight;

		[SerializeField] private float _aimDuration = 0.2f;
		[SerializeField] private float _drawDuration = 0.5f;

		[SerializeField] private float _timeLerp = 0.2f;

		public bool _isStartCourutine = true;
		public bool _drawWeapon = false;
		public bool _hideWeapon = false;

		[SerializeField] private Transform rightHolderPosition;
		[SerializeField] private Transform leftHolderPosition;



		private void Start()
		{
			animator = GetComponent<Animator>();
			weaponController = GetComponent<WeaponController>();

			EventManager.Aim += OnAim;
			EventManager.CurrentWeapon += OnCurrentWeapon;
		}

		
		#region EventManager
		private void OnAim(bool obj)
		{
			
		}
		private void OnCurrentWeapon(int obj)
		{
			
			if(obj == 0)
				_hideWeapon = true;
			else
				_drawWeapon = true;
		}
		#endregion

		private void Update()
		{
			DrawWeapon();
			HideWeapon();
		}

		private void HideWeapon()
		{
			if (_hideWeapon && !_drawWeapon)
			{
				weaponController.IsEquip = false;
				TransformLerp(rightHandTarget, rightHolderPosition, _drawDuration);
				TransformLerp(leftHandTarget, leftHolderPosition, _drawDuration);
				if (_isStartCourutine)
				{
					_isStartCourutine = false;
					StartCoroutine(HideDrawWeapon());
				}
				else
				{
					if (_rightHandWeight > 0f)
					{
						_rightHandWeight -= Time.deltaTime / _aimDuration;
						_leftHandWeight -= Time.deltaTime / _aimDuration;
					}
				}
			}
		}

		private void DrawWeapon()
		{
			if (_drawWeapon && !_hideWeapon)
			{
				if (_rightHandWeight < 1f)
				{
					_leftHandWeight += Time.deltaTime / _aimDuration;
					_rightHandWeight += Time.deltaTime / _aimDuration;
				}

				if (_rightHandWeight > 0.9f)
				{
					TransformLerp(rightHandTarget, rightHand, _drawDuration);
					TransformLerp(leftHandTarget, leftHand, _drawDuration);
					if (_isStartCourutine)
					{
						weaponController.IsEquip = true;
						weaponController.EquipWeapon(true);
						_isStartCourutine = false;
						StartCoroutine(HideDrawWeapon());
					}
				}
			}
		}
		private IEnumerator HideDrawWeapon()
		{
			
			yield return new WaitForSeconds(_timeLerp);
			_drawWeapon = _drawWeapon ? false : _drawWeapon;
			if(_hideWeapon)
			{
				weaponController.EquipWeapon(false);
				_hideWeapon = false;
				_rightHandWeight = 0f;
				_leftHandWeight = 0f;
			}
			_isStartCourutine = true;
		}

		private void TransformLerp(Transform a, Transform b, float time)
		{
			a.position = Vector3.Lerp(a.position,b.position, time);
			a.rotation = Quaternion.Lerp(a.rotation,b.rotation, time); 
		}
		private void OnAnimatorIK(int layerIndex)
		{
			animator.SetLookAtPosition(lookAtPosition.position);
			animator.SetLookAtWeight(_lookWeight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
			//LeftHand
			if (leftHand != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftHandWeight);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftHandWeight);

				animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

				animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftHint.position);
				animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, _leftHandWeight);
			}

			//RightHand
			if (rightHand != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandWeight);
				animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);

				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightHandWeight);
				animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

				animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightHint.position);
				animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, _rightHandWeight);
			}



		}
		private void OnDestroy()
		{
			EventManager.Aim -= OnAim;
			EventManager.CurrentWeapon -= OnCurrentWeapon;
		}
	}
}

