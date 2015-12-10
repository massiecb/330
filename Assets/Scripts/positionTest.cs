using UnityEngine;
using System.Collections;

public class positionTest : MonoBehaviour {

	// Use this for initialization
	//private Rigidbody body;
	void Start () {
		//body = GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider other){
		other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}
}
