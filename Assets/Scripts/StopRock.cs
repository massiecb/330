using UnityEngine;
using System.Collections;

public class StopRock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){
		Debug.Log ("Rocksin");
		if (other.gameObject.tag.Equals ("OriginRock"))
			Debug.Log ("true");
			other.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
	}
}
