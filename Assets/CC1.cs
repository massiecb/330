using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CC1 : MonoBehaviour {
	
	public Vector3 lane;
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, angle, road_smooth;
	public Transform[] ctrlA;
	public Transform[] ctrlB;
	public BSpline[] BS;
	public Transform[][] array; 
	private BSpline b;
	private BSpline b2;
	private int whatTrack;
	private int trackMin = 0;
	private int trackMax = 1;
	// Use this for initialization
	void Start () {
//		Debug.Log (b);
		b = new BSpline(ctrlA);
		b2 = new BSpline (ctrlB);
		//for (int i = 0; i < b.ctlPoint.Length; i++)
		//	Debug.Log (b.ctlPoint [i].position);
		BS = new BSpline[] {
		//new BSpline(ctrlA) , 
			b,
			b2};
		//new BSpline(ctrlA)};
		//Debug.Log (BS [2]);
//		for (int i = 0; i < BS.Length; i++)
//			Debug.Log (BS[i]);
			//new BSpline(ctrlB)};
		array = new Transform[][] {
			 new Transform[ctrlA.Length],
			new Transform[ctrlB.Length]
		};
//		for (int i = 0; i < ctrlA.Length; i++)
//			Debug.Log (ctrlA[i].position);
		//b = BS [0];
		//b2 = BS [1];
		array [0] = ctrlA;
		array [1] = ctrlB;
		//BS [0] = b;
		//BS [1] = b2;
		whatTrack = 0;
//		Debug.Log (BS [whatTrack]);
		u = 0f;
		top_speed = 40f;
		//        road_smooth = 0.1f;
		velocity = 0;
		lane = Vector3.zero;
		drag *= weight;
			
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log ("FU(0): " + u + " V= " + velocity);
		//changeLanes(); // after net_pos
		//deltaVelocity();
		velocity = 1;
		//Debug.Log ("FU(1): " + u + " V= " + velocity);
		u += Time.fixedDeltaTime * velocity / 5;

		if (u >= BS[whatTrack].Length)
			u -= BS[whatTrack].Length;
//		Vector3 next_pos = BS[whatTrack].Evaluate(u);
//		Vector3 next_pos = b.Evaluate (u);
		Vector3 next_pos = changeLanes ();
		//changeLanes (next_pos);
		transform.LookAt(next_pos);
		transform.position = next_pos + lane;        
	}
	
	void deltaVelocity() {
		//Debug.Log ("DV: " + drag + " " + first_gear + " " + second_gear + " " + third_gear + " " + fourth_gear + " " + top_speed);
		if (CrossPlatformInputManager.GetAxis("Vertical") == 0 && velocity > 0)
			velocity -= drag;
		else if (velocity <= 10)
			velocity += CrossPlatformInputManager.GetAxis("Vertical")/first_gear;
		else if (velocity <= 20)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / second_gear;
		else if (velocity <= 30)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / third_gear;
		else if (velocity < top_speed)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / fourth_gear;
		else
			velocity = top_speed;
		if (velocity < 0)
			velocity = 0;
	}
	
	Vector3 changeLanes() {
		//Debug.Log (CrossPlatformInputManager.GetAxis ("Horizontal"));
		// in this the size is 2
		Vector3 nextPosition = BS [whatTrack].Evaluate (u);
		//foreach (Transform t in array[whatTrack])
		//	Debug.Log (t.position);
		//for (int i = 0; i < BS[whatTrack].ctlPoint.Length; i++)
	//		Debug.Log (BS [whatTrack].ctlPoint [i].position);
		//Debug.Log (nextPosition);
		//float searchX;
		//float searchZ;
		//int lowestIndex = 0;

		if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0 && whatTrack != trackMin) {
			Debug.Log("one");
			whatTrack -= 1;
			//searchX = transform.position.x / 2f;
			//searchZ = transform.position.z / 2f;
			for (int i = 0; i < array[whatTrack].Length - 1; i++){ 
				//Debug.Log ("bob");
				Debug.Log (array[whatTrack][i+1].position.x);
				//if ((Mathf.Abs(array[whatTrack][i+1].position - nextPosition)) > (Mathf.Abs (array[whatTrack][i].position - nextPosition))){
				if ((Mathf.Abs (array[whatTrack][i+1].position.x - nextPosition.x)) > (Mathf.Abs(array[whatTrack][i].position.x - nextPosition.x))){
					if ((Mathf.Abs(array[whatTrack][i+1].position.z - nextPosition.z)) > (Mathf.Abs(array[whatTrack][i].position.z - nextPosition.z))){
						Debug.Log ("in loop" + array[whatTrack][i].position);
					 	nextPosition = array[whatTrack][i].position;
					}
				}
				else if (i == array[whatTrack].Length -1){
					nextPosition = array[whatTrack][0].position;
					Debug.Log ("overflow: " + nextPosition);
				}
			}
		} 
		else if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0 && whatTrack != trackMax) {
			Debug.Log ("two");
			whatTrack +=1;
			//searchX = transform.position.x / 2f;
			//searchZ = transform.position.z /2f;
			for (int i = 0; i < array[whatTrack].Length; i++){
				//Debug.Log ("AntiBob");
				//if ((Mathf.Abs(array[whatTrack][i+1].position - nextPosition)) > (Mathf.Abs (array[whatTrack][i].position - nextPosition))){
				//	nextPosition = array[whatTrack][i].position;
				//}
				if ((Mathf.Abs (array[whatTrack][i+1].position.x - nextPosition.x)) > (Mathf.Abs(array[whatTrack][i].position.z - nextPosition.z))){
					if ((Mathf.Abs(array[whatTrack][i+1].position.z - nextPosition.z)) > (Mathf.Abs(array[whatTrack][i].position.z - nextPosition.z))){
						Debug.Log (array[whatTrack][i].position);
						nextPosition = array[whatTrack][i].position;
					}
				}
			}
		}
		//if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0)
		//	Debug.Log ("greater than");
		//else if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0 && whatTrack != trackMax)
		//	Debug.Log ("less than");

		//Debug.Log (nextPosition);
		return nextPosition;
	}
}