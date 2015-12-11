using UnityEngine;
using System.Collections;

public class GreenShell : MonoBehaviour {
	public Vector3 speed;
	public float timer;
		
	void Start () {
		timer = Time.time;
	}
	
	void Update () {
		transform.position += Time.deltaTime * speed;
		var timeAlive = Time.time - timer;

		if (timeAlive > 10) {
			Destroy(this.gameObject);
		}
	}
}
