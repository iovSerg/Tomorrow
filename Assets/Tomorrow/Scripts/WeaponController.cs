using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomorrow
{
	public class WeaponController : MonoBehaviour
	{
		[SerializeField] private GameObject rightHolder;
		[SerializeField] private GameObject leftHolder;

		[SerializeField] private GameObject leftWeapon;
		[SerializeField] private GameObject rightWeapon;
		private void Start()
		{
			
		}
		public void EquipWeapon(bool state)
		{
			rightHolder.SetActive(!state);
			leftHolder.SetActive(!state);
			leftWeapon.SetActive(state);
			rightWeapon.SetActive(state);
		}
	}
}
