using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    private Camera mainCamera;
    Ray ray;
    RaycastHit hit;

	private void Start()
	{
		mainCamera = Camera.main;
	}
	private void Update()
	{
		ray.origin = mainCamera.transform.position;
		ray.direction = mainCamera.transform.forward;
		Physics.Raycast(ray, out hit);
		transform.position = hit.point;
	}
}
