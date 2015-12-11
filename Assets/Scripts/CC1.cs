using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class CC1 : MonoBehaviour {
    //force commit
    private GameObject ghost;
	public float u, velocity, first_gear, second_gear, third_gear, fourth_gear, top_speed, drag, lane_change, shift;
	private BSpline bspline;
	private int whatTrack, trackMin = -1, trackMax = 1, lane_change_pause = 0, lap_count;
    Terrain gameTerrain;
    Text race_status;
    Image power_up;
    public Sprite oil_texture, missile_texture, speed_texture, invincible_texture;
    public enum Avail_PowerUps {None, Oil, GreenShell, SpeedBoost, Invincible, RedShell};
	public Avail_PowerUps powerUp = CC1.Avail_PowerUps.None;
	private SpawnPoint spawnPoint;
	public Transform spawnFwd, spawnBck;
	public bool winpage;
	private int laps;
	// Use this for initialization

	void Awake(){
		spawnPoint = transform.FindChild ("spawnPointFront").GetComponent<SpawnPoint> ();
		spawnFwd = transform.FindChild ("spawnPointFront");
		spawnBck = transform.FindChild ("spawnPointBack");
	}

    void Start () {
        GameObject track = GameObject.FindGameObjectWithTag("TrackB");
        gameTerrain = Terrain.activeTerrain;
        bspline = new BSpline (track.GetComponentsInChildren<Transform>());
        whatTrack = 0;
		u = 1f;
        ghost = new GameObject();
		top_speed = 120f;
		velocity = 0f;
        shift = 4f;
        lap_count = 1;
        power_up = UnityEngine.UI.Image.FindObjectOfType<Image>();
        power_up.enabled = false;
        race_status = UnityEngine.UI.Text.FindObjectOfType<Text>();
        drag = 1;
        lane_change = 0f;
        first_gear = 6; second_gear = 5; third_gear = 4; fourth_gear = 2;
        for (int i = 0; i < track.transform.childCount; i++)
            track.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
		winpage = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (lap_count < 2 && winpage == false) {
			velocity = deltaVelocity (velocity);
			float next_position;
			if (lane_change_pause < 15)
				lane_change_pause++;
			if (u + 1 >= bspline.Length)
				next_position = u + 1 - bspline.Length;
			else
				next_position = (float)Math.Truncate (u) + 1f;
			float point_smooth = 1 / (bspline.Evaluate (next_position) - bspline.Evaluate ((float)Math.Truncate (u))).magnitude;
			u += Time.fixedDeltaTime * velocity / 5 * point_smooth;
			if (CrossPlatformInputManager.GetAxis ("Horizontal") != 0 && lane_change_pause == 15)
				changeLanes ();
			if (u >= bspline.Length) {
				u -= bspline.Length;
				lap_count += 1;
			}
			Vector3 next_pos = bspline.Evaluate (u);
			UI_Update ();
			ghost.transform.LookAt (next_pos);
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
			if (u < 76 || u > 87)
				next_pos.y = gameTerrain.SampleHeight (next_pos) + 0.3f;
			transform.localRotation = ghost.transform.localRotation;
			transform.position = transform.position * (1 - lane_change) + next_pos * lane_change;
			if (Input.GetKeyDown ("space") && powerUp != Avail_PowerUps.None) {
				//Debug.Log (powerUp);
				power_up.enabled = false;
				spawnPoint.UsePowerUp (powerUp);
				powerUp = Avail_PowerUps.None;
				//            Debug.Log(powerUp);
			}
		}
			else if (winpage == false){
				print ("You WIN!!!!!");
				winpage = true;
			}
	}

    private float deltaVelocity(float velocity) {
		//Debug.Log ("DV: " + drag + " " + first_gear + " " + second_gear + " " + third_gear + " " + fourth_gear + " " + top_speed);
		if (CrossPlatformInputManager.GetAxis("Vertical") == 0 && velocity > 0)
			velocity -= drag;
		else if (velocity <= 20)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / first_gear;
		else if (velocity <= 40)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / second_gear;
		else if (velocity <= 80)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / third_gear;
		else if (velocity < top_speed)
			velocity += CrossPlatformInputManager.GetAxis("Vertical") / fourth_gear;
		else
			velocity = top_speed;
		if (velocity < 0)
			velocity = 0;
        return velocity;
	}
	
	public void changeLanes() {
        lane_change = 0;
        lane_change_pause = 0;
        if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0 && whatTrack > trackMin)
			whatTrack -= 1;
		else if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0 && whatTrack < trackMax)
            whatTrack +=1;
	}

    public void UI_Update() {
        race_status.text = "Lap: " + (lap_count).ToString() + "\n" + "Velocity: " + velocity.ToString("####0.00") + "\n" + "Lane: " + (whatTrack + 2);
        if (powerUp != Avail_PowerUps.None) {
            power_up.enabled = true;
            switch (powerUp) {
                case Avail_PowerUps.Oil:
                    power_up.sprite = oil_texture;
                    break;
                case Avail_PowerUps.GreenShell:
                    power_up.sprite = missile_texture;
                    break;
                case Avail_PowerUps.SpeedBoost:
                    power_up.sprite = speed_texture;
                    break;
                case Avail_PowerUps.Invincible:
                    power_up.sprite = invincible_texture;
                    break;
            }
        }
    }

    public void OnCollisionEnter(Collision thing){
        switch (thing.gameObject.tag) {
            case "OriginRock":
                if (whatTrack == 1 || whatTrack == 0)
                    whatTrack -= 1;
                else if (whatTrack == -1)
                    whatTrack += 1;
                velocity = 0;
                break;
            case "missile":
				whatTrack = UnityEngine.Random.Range (-1, 1);
				velocity -= 30;
                break;


        }
    }
    public void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("oil"))
			top_speed -= 30;
		else if (other.gameObject.CompareTag ("Pick Up") && powerUp == Avail_PowerUps.None) {
			int i = UnityEngine.Random.Range (1, 4);
			powerUp = (Avail_PowerUps)Enum.GetValues (typeof(Avail_PowerUps)).GetValue (i);
		}
    }
    public void OnTriggerExit(Collider other){
        if (other.tag.Equals("oil"))
            top_speed += 30;
        }
	}