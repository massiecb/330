using UnityEngine;
using System.Collections;

//public enum Avail_PowerUps { Oil, GreenShell, RedShell, SpeedBoost, Invincible };

public class SpawnPointAI : MonoBehaviour {
	public GameObject obj;
	public float timer;
	private AI ctrl;
	public Texture oil_texture, missile_texture;
	
	
	void Start() {
		ctrl = transform.parent.GetComponent<AI>();
	}
	public void UsePowerUp(CC1.Avail_PowerUps item) {
		switch(item)	{
		case CC1.Avail_PowerUps.GreenShell:
			GreenShell();
			break;
		case CC1.Avail_PowerUps.Oil:
			Oil();
			break;
		case CC1.Avail_PowerUps.SpeedBoost:
			StartCoroutine (SpeedBoost());
			break;
		case CC1.Avail_PowerUps.Invincible:
			StartCoroutine(Invincible());
			break;
		}	
	}
	
	void GreenShell() {
		GameObject instantiatedObj = Instantiate (obj, ctrl.spawnFwd.position, transform.rotation) as GameObject;
		instantiatedObj.name = "GreenShell";
		instantiatedObj.gameObject.AddComponent<GreenShell>();
		instantiatedObj.tag = "missile";
		instantiatedObj.AddComponent<BoxCollider>();
		instantiatedObj.GetComponent<Renderer>().material.mainTexture = missile_texture;
		GreenShell shell = instantiatedObj.gameObject.GetComponent<GreenShell>();
		shell.speed = transform.forward * 50f;
		Object.Destroy(instantiatedObj, 15f);
	}
	void Oil() {
		GameObject instantiatedObj = Instantiate(obj, ctrl.spawnBck.position, transform.rotation) as GameObject;
		instantiatedObj.transform.localScale = new Vector3(4f, 0.000001f, 4f);
		instantiatedObj.name = "oil";
		instantiatedObj.gameObject.tag = "oil";
		instantiatedObj.AddComponent<BoxCollider>();
		instantiatedObj.GetComponent<BoxCollider>().isTrigger = true;
		instantiatedObj.GetComponent<Renderer>().material.mainTexture = oil_texture;
		Object.Destroy(instantiatedObj, 30f);
	}
	IEnumerator SpeedBoost(){
		ctrl.top_speed = 150;
		ctrl.velocity = ctrl.top_speed;
		yield return new WaitForSeconds (5);
		ctrl.top_speed = 120;
		if (ctrl.velocity > 120)
			ctrl.velocity = ctrl.top_speed;
	}
	IEnumerator Invincible(){
		ctrl.gameObject.transform.GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds (10);
		ctrl.gameObject.transform.GetComponent<Collider>().enabled = true;
	}
}