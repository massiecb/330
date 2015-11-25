using UnityEngine;
using System.Collections;

public class FlyingRocks : MonoBehaviour {

	// Use this for initialization
	public GameObject rock;
	public GameObject OriginRock;
	public Vector3 rockThrow;
	public Transform[] trackA;
	public Transform[] trackC;
	public Transform[] rocks;
	private int rockNumber = 0;
	void Start () {
		OriginRock = GameObject.FindWithTag ("OriginRock");
		trackA = GameObject.FindWithTag ("TrackA").GetComponentsInChildren<Transform> ();
		trackC = GameObject.FindWithTag ("TrackC").GetComponentsInChildren<Transform> ();
		rocks = new Transform[20];
		for (int i = 0; i < 20; i++) {
			int coinFlip = Random.Range (0, 1);
			if (coinFlip > 0){
				int position = Random.Range (0, trackA.Length);
				rocks[i] = trackA[position];
			}
			else{
				int position = Random.Range (0, trackA.Length);
				rocks[i] = trackC[position];
			}
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (rockNumber < rocks.Length) {
			rock = Instantiate (OriginRock);
			rock.GetComponent<Renderer>().enabled = true;
			rock.GetComponent<SphereCollider>().enabled = true;
			rockThrow = ThrowRock (OriginRock.transform, rocks [rockNumber]);
			//rock.GetComponent<Rigidbody>().AddForce (rockThrow, ForceMode.VelocityChange);
			rock.transform.position = rocks[rockNumber].position;
			rockNumber += 1;
			//rock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}

	}

	private Vector3 ThrowRock (Transform origin, Transform target){
		//use basic phyiscs formuals to calculate
		float yInitialVelocity;
		float xInitialVelocity;
		float zInitialVelocity;
		Vector3 setTarget;
		float airTime = 20f;
		Vector3 toTarget = origin.position - target.position;
		//initial y speed = distance/ 1/2 gt^2
		yInitialVelocity = toTarget.y / airTime + 0.5f * Physics.gravity.magnitude * airTime * airTime;
		//horizontal have no acceleration v = d/t
		xInitialVelocity = toTarget.x / airTime;
		zInitialVelocity = toTarget.z / airTime;
		setTarget = toTarget.normalized; // set direction
		setTarget.x = xInitialVelocity;
		setTarget.z = zInitialVelocity;
		setTarget.y = yInitialVelocity;
		return setTarget;
	}
}
