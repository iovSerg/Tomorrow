using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tomorrow
{
	public class PlayerInput : MonoBehaviour
	{
		private PlayerInputSystem input;
		[SerializeField] private Vector2 _move;
		[SerializeField] private Vector2 _look;
		[SerializeField] private float _magnituda;
		[SerializeField] private bool _sprint;
		[SerializeField] private bool _rool;
		[SerializeField] private bool _crouch;

		[SerializeField] private int _currentWeapon;
		[SerializeField] private bool _aim = false;
		[SerializeField] private bool _fire = false;
		public Vector2 Move => _move;
		public Vector2 Look => _look;
		public bool Sprint => _sprint;
		public bool Roll => _rool;
		public bool Crouch => _crouch;
		public bool Aim => _aim;
		public bool Fire => _fire;
		public float Magnituda => _magnituda;
		public int Weapon =>_currentWeapon;

		private void Awake()
		{
			if (input == null)
				input = new PlayerInputSystem();

			input.Player.Move.performed += i => _move = i.ReadValue<Vector2>();
			input.Player.Move.canceled += i => _move = i.ReadValue<Vector2>();

			input.Player.Sprint.performed += i => _sprint = i.ReadValueAsButton();

			input.Player.Look.performed += i => _look = i.ReadValue<Vector2>();
			input.Player.Look.canceled += i => _look = i.ReadValue<Vector2>();

			input.Player.Roll.performed += i => _rool = i.ReadValueAsButton();
			input.Player.Roll.canceled += i => _rool = i.ReadValueAsButton();

			input.Player.Crouch.performed += i => _crouch = i.ReadValueAsButton();
			input.Player.Crouch.canceled += i => _crouch = i.ReadValueAsButton();

			input.Player.Aim.performed += i => { _aim = !_aim; EventManager.Aim?.Invoke(_aim); };

			input.Player.Fire.performed += i => {
				_fire = i.ReadValueAsButton();
				_aim = _fire;
				EventManager.Aim?.Invoke(_aim);
				EventManager.Fire?.Invoke(_fire);
			};
			input.Player.Fire.canceled += i => { _fire = i.ReadValueAsButton(); EventManager.Fire?.Invoke(_fire); };

			input.Player.Weapon.performed += OnSetWeapon;

		}

		private void OnSetWeapon(InputAction.CallbackContext context)
		{
			int weapon = 0;
			int.TryParse(context.control.displayName, out weapon);

			if (_currentWeapon == weapon) _currentWeapon = 0;
			else _currentWeapon = weapon;

			EventManager.CurrentWeapon?.Invoke(_currentWeapon);

		}

		private void Update()
		{
			_magnituda = Mathf.Clamp01(Mathf.Abs(Move.x) + Mathf.Abs(Move.y));
		}
		private void OnEnable()
		{
			input.Enable();
		}
		private void OnDisable()
		{
			input.Disable();
		}
	}
}

