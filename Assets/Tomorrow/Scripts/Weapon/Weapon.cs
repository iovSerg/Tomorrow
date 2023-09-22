using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomorrow
{
	public class Weapon : MonoBehaviour
	{
		public ParticleSystem[] muzzleFlash;
		public ParticleSystem hitEffect;
		public Transform rayCastOrigin;
		public Transform rayCastTarget;
		public WeaponData data;

		Ray ray;
		RaycastHit hit;
		private void Start()
		{
			
			muzzleFlash = GetComponentsInChildren<ParticleSystem>();
			rayCastTarget = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<CrossHairTarget>().transform;
			rayCastOrigin = gameObject.transform.Find("RayCast");
			hitEffect = GameObject.Instantiate(data.metallEffect,transform);
		}
		public void Fire()
		{
			foreach (var weapon in muzzleFlash)
			{
					weapon.Emit(1);
			}

			ray.origin = rayCastOrigin.transform.position;
			ray.direction = rayCastTarget.position - rayCastOrigin.transform.position;

			if(Physics.Raycast(ray,out hit))
			{
				

				hitEffect.transform.position = hit.point;
				hitEffect.transform.forward = hit.normal;
				hitEffect.Emit(1);
			}

		}
	}
	
}
