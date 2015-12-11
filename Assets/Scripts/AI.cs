using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour {

	// Use this for initialization
	private Animator anim;
	private GameObject ghost;
	//public enum Avail_PowerUps{Oil, GreenShell, RedShell, SpeedBoost, Invincible};
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, weight, lane_change, shift;
	private BSpline b;
	public int whatTrack;
	private int trackMin = -1, trackMax = 1, lane_change_pause = 0;
	//public bool hasShell = false, hasOilSlick = false, enemyInFront = false, rockInFront = false; 
	public CC1.Avail_PowerUps powerUp;
	private SpawnPointAI spawnPoint;
	public Transform spawnFwd, spawnBck;
	public bool winpage;
	private int delay = 0;
	private int lap_count;

	void Awake () {
		spawnPoint = transform.FindChild ("spawnPointFront").GetComponent<SpawnPointAI> ();
		spawnFwd = transform.FindChild ("spawnPointFront");
		spawnBck = transform.FindChild ("spawnPointBack");
	}

	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Accel", true);
		GameObject track = GameObject.FindGameObjectWithTag ("TrackB");
		b = new BSpline (track.GetComponentsInChildren<Transform>());
		whatTrack = 0;
		u = 1f;
		ghost = new GameObject();
		top_speed = 120f;
		velocity = 0f;
		shift = 4.0f;
		drag = 2; weight = 10; drag *= weight;
		lane_change = 0f;
		first_gear = 10; second_gear = 8; third_gear = 6; fourth_gear = 4;
		winpage = false;
		lap_count = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if ( lap_count < 4 && winpage == false){
			AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0);
			if (delay < 15)
				delay++;
			velocity = deltaVelocity (velocity);
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
			if (u >= b.Length){
				u -= b.Length;
				lap_count++;
			}
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
			else if (state.IsName ("UseShell")) {
				if (delay == 15)
					ShellHandler ();
				delay = 0;
			}
			else if (state.IsName ("ChangeLanes"))
				ChangeLane ();
			else if (state.IsName ("UseOilSlick"))
				OilSlickHandler ();
			else if (state.IsName ("Speedy"))
				SpeedyHandler ();
			else if (state.IsName ("Invinc"))
				InvincHandler ();
		}
		else if (winpage == false){
			GameObject [] temp = GameObject.FindGameObjectsWithTag("Car");
			for (int i = 0;  i < temp.Length; i++){
				if (temp[i].GetComponent<CC1>() != null)
					temp[i].GetComponent<CC1>().winpage = true;
			}
			winpage = true;
			print ("You lose");
		}
	}

	private void AccelerationHandler(){
		//check to see if we have item. Similar to idle;
	}

	private void ShellHandler(){
		Animator anim = gameObject.GetComponent<Animator> ();
		//if we have shell and enemy in range, use it regardless of type
		spawnPoint.UsePowerUp (powerUp);
		anim.SetBool ("HasShell", false);
	}

	private void OilSlickHandler(){
		//just use once have
		Animator anim = gameObject.GetComponent<Animator> ();
		spawnPoint.UsePowerUp (powerUp);
		anim.SetBool ("HasOilSlick", false);
	}

	private void SpeedyHandler(){
		Animator anim = gameObject.GetComponent<Animator> ();
		spawnPoint.UsePowerUp (powerUp);
		anim.SetBool ("HasSpeed", false);
	}

	private void InvincHandler(){
		Animator anim = gameObject.GetComponent<Animator> ();
		spawnPoint.UsePowerUp (powerUp);
		anim.SetBool ("HasIninc", false);
	}

	private void ChangeLane(){
		Animator anim = gameObject.GetComponent<Animator> ();
		transform.GetComponent<CapsuleCollider> ().isTrigger = true;
		//if (whatTrack == 1 || whatTrack == 0)
		if (whatTrack == trackMax)
			whatTrack -= 1;
		else if (whatTrack == trackMin)
			whatTrack += 1;
		else if (whatTrack == 0) {
			int coinFlip = UnityEngine.Random.Range (0,1);
			if (coinFlip == 0)
				whatTrack += 1;
			else if (coinFlip == 1)
				whatTrack -=1;
		}
		pause ();
		velocity = 0f;
		anim.SetBool("RockinFront", false);
		transform.GetComponent<CapsuleCollider> ().isTrigger = false;
	}

	private void OnCollisionEnter (Collision other){
		switch (other.gameObject.tag) {
			case "OriginRock":
				anim.SetBool("RockinFront", true);
				break;	
			case "missile":
				whatTrack = UnityEngine.Random.Range (-1, 1);
				if (velocity < 30)
					velocity = 0;
				else
					velocity -= 30;
				break;
		}

	}

	private void OnTriggerEnter (Collider other){
		if (other.tag.Equals ("oil")) {
			top_speed = 60;
			if (velocity < 30)
				velocity = 0f;
			else
				velocity -= 30;
		}
		else if (other.gameObject.CompareTag ("Pick Up")  && powerUp == CC1.Avail_PowerUps.None) {
			int i = UnityEngine.Random.Range (1, 4);
			powerUp = (CC1.Avail_PowerUps)Enum.GetValues (typeof(CC1.Avail_PowerUps)).GetValue (i);
			if (powerUp == CC1.Avail_PowerUps.GreenShell){
				anim.SetBool("HasShell", true);
				//anim.SetBool ("EnemyFront", true);
			}
			if (powerUp == CC1.Avail_PowerUps.Invincible){
				anim.SetBool("HasInvinc", true);
				//anim.SetBool ("EnemyFront", true);
			}
			if (powerUp == CC1.Avail_PowerUps.Oil){
				anim.SetBool("HasOilSlick", true);
				//anim.SetBool ("EnemyFront", true);
			}
			if (powerUp == CC1.Avail_PowerUps.SpeedBoost){
				anim.SetBool("HasSpeed", true);
				//anim.SetBool ("EnemyFront", true);
			}
		}

	}

	private float deltaVelocity(float velocity) {/*
		 if (velocity <= 15) {
			velocity += .1f;
		} else if (velocity <= 35)
			velocity += .4f;
		else if (velocity <= 55)
			velocity += .8f;
		else if (velocity <= 85)
			velocity += 1f;
		else if (velocity <= 105)
			velocity += 1.2f;
		else if (velocity <= top_speed)
			velocity = top_speed;
		*/

		 if (velocity <= 20) {
			velocity += 1 / first_gear;
		} else if (velocity <= 40) {
			velocity += 1 / second_gear;
		} else if (velocity <= 80) {
			velocity += 1 / third_gear;
		} else if (velocity < top_speed) {
			velocity += 1 / fourth_gear;
		}
		else
			velocity = top_speed;
		if (velocity < 0)
			velocity = 0;
		return velocity;
	}

	IEnumerator pause(){
		yield return new WaitForSeconds (2f);
	}

	public void OnTriggerExit(Collider other){
		if (other.tag.Equals("oil"))
			top_speed = 120;
	}
}
