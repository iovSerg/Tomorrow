using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public GameObject[] part;
	public GameObject armature;
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		armature.SetActive(false);
		foreach (var p in part)
			p.SetActive(true);
	}
}
