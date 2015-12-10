using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	public Transform s;
	void Start () {
		s = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 origin = new Vector3 (-2.3f, 5.51f, -2.32f);
		Vector3 vec = s.position - new Vector3(-2.3f, 5.51f, -2.32f);
		float d = vec.sqrMagnitude;
		Vector3 apply = new Vector3 (1, 0, 0) * d/10f;
		//float zAngle =  Vector3.Angle (new Vector3 (0, 0, s.position.z),  new Vector3 (0, 0, origin.z));
		float zAngle = Mathf.Acos ((Vector3.Cross (s.position, origin).magnitude) / (s.position.magnitude * origin.magnitude));
		//apply.z = 1;
		apply.z = zAngle * 2f;// Mathf.Deg2Rad;// * zAngle;
		//float yAngle = Vector3.Angle (new Vector3 (0, s.position.y, 0), new Vector3 (0, origin.y, 0));
		//Debug.Log (apply.y);
		//apply.y = Mathf.Deg2Rad * yAngle;
		float yAngle = Mathf.Asin ((Vector3.Cross (s.position, origin).magnitude) / (s.position.magnitude * origin.magnitude));
		//Debug.Log (Mathf.Round(yAngle));
		apply.y = yAngle * 2f;
		this.GetComponent<Rigidbody> ().AddForce (apply, ForceMode.VelocityChange);
	
	}
}
