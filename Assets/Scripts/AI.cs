using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour {

	// Use this for initialization
	private Animator anim;
	private GameObject ghost;
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, lane_change, shift;
	private BSpline b;
	public int whatTrack;
	private int trackMin = -1, trackMax = 1, lane_change_pause = 0;
	public bool hasShell = false, hasOilSlick = false, enemyInFront = false, rockInFront = false;
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Accel", true);
		GameObject track = GameObject.FindGameObjectWithTag ("TrackB");
		b = new BSpline (track.GetComponentsInChildren<Transform>());
		whatTrack = 0;
		u = 1f;
		ghost = new GameObject();
		top_speed = 120f;
		velocity = 1f;
		shift = 4.0f;
		drag = 2; weight = 10; drag *= weight;
		lane_change = 0f;
		first_gear = 10; second_gear = 8; third_gear = 6; fourth_gear = 4;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0);
		if (lane_change_pause < 15)
			lane_change_pause++;
		float next_position;
		if (u + 1 > b.Length)
			next_position = u + 1f - b.Length;
		else
			next_position = (float)Math.Truncate (u) + 1f;
		float point_smooth = 1 / (b.Evaluate(next_position) - b.Evaluate((float)Math.Truncate(u))).magnitude;
		u += Time.fixedDeltaTime * velocity / 5 * point_smooth;
		//if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 && lane_change_pause == 15)
		//	changeLanes();
		if (u >= b.Length)
			u -= b.Length;
		Vector3 next_pos = b.Evaluate(u);
		ghost.transform.LookAt(next_pos);
		ghost.transform.position = next_pos;
		if (whatTrack == -1)
			next_pos = ghost.transform.position - ghost.transform.right * shift;
		else if (whatTrack == 1)
			next_pos = ghost.transform.position + ghost.transform.right * shift;
		if (lane_change != 1) {
			if (lane_change + 0.01 > 1f)
				lane_change = 1;
			else
				lane_change += 0.01f;
		}
		transform.localRotation = ghost.transform.localRotation;
		transform.position = transform.position * (1 - lane_change) + next_pos * lane_change;
		if (state.IsName ("Accelerate"))
			AccelerationHandler ();
		else if (state.IsName ("UseShell"))
			ShellHandler ();
		else if (state.IsName ("ChangeLanes"))
			ChangeLane ();
		else if (state.IsName ("UseOilSlick"))
			OilSlickHandler ();

	}

	private void AccelerationHandler(){
		//check to see if we have item. Similar to idle
	}

	private void ShellHandler(){
		//if we have shell and enemy in range, use it regardless of type
	}

	private void ChangeLane(){
		// if rock in way, change lanes
	}

	private void OilSlickHandler(){
		//just use once have
	}

	private void onTriggerEnter (Collider collision){
		switch (collision.gameObject.tag){
			case "OriginRock":
				anim.SetBool("RockinFront", true);
				break;
			case "CarTag":
				anim.SetBool ("EnemyFront", true);
				break;
		}
	}
}
