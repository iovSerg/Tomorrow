using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomorrow
{
	public class WeaponController : MonoBehaviour
	{
		[SerializeField] private GameObject rightHolder;
		[SerializeField] private GameObject leftHolder;

		[SerializeField] private GameObject leftHandWeapon;
		[SerializeField] private GameObject rightHandWeapone;

		public Weapon rightWeapon;
		public Weapon leftWeapon;

		private bool _isFire= false;
		private bool _isEquip = false;

		public bool IsEquip { get=>_isEquip; set=>_isEquip = value; }
		private void Start()
		{
			EventManager.Fire += OnFire;
		}

		private void Update()
		{
			if(_isFire && _isEquip)
			{
				if(rightWeapon && leftWeapon)
				{
					rightWeapon.Fire();
					leftWeapon.Fire();
				}
			}
		}

		private void OnFire(bool obj)
		{
			_isFire = obj;
		}

		public void EquipWeapon(bool state)
		{
			rightHolder.SetActive(!state);
			leftHolder.SetActive(!state);

			leftHandWeapon.SetActive(state);
			leftWeapon = leftHandWeapon.GetComponent<Weapon>();
			rightHandWeapone.SetActive(state);
			rightWeapon = rightHandWeapone.GetComponent<Weapon>();
		}
	}
}
