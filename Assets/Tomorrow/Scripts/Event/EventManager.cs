using System;
using UnityEngine;

	public  class EventManager : MonoBehaviour
	{
		public static Action<bool> Aim {  get; set; }
		public static Action<bool> Fire {  get; set; }
		public static Action<int> CurrentWeapon { get; set; }
	}

