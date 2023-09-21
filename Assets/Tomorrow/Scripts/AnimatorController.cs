using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomorrow
{
	public class AnimatorController : MonoBehaviour
	{
		private Animator animator;
		private PlayerInput playerInput;
		private float _animationBlend;

		public float SpeedChangeRate = 10f;

		private void Start()
		{
			animator = GetComponent<Animator>();
			playerInput = GetComponent<PlayerInput>();	
		}

		private void Update()
		{
			Movement();
		}

		private void Movement()
		{
			float magnituda = playerInput.Sprint ? playerInput.Magnituda / 2 : playerInput.Magnituda;

			_animationBlend = Mathf.Lerp(_animationBlend, magnituda, Time.deltaTime * SpeedChangeRate);

			if (_animationBlend < 0.01f) _animationBlend = 0f;
		
			animator.SetFloat("Speed", _animationBlend);
			
		}
	}
}
