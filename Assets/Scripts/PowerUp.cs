using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Car")) {
			StartCoroutine (RespawnPowerUp(2f));
		}
	}	

	IEnumerator RespawnPowerUp(float sec) {
		GameObject child = transform.FindChild("Display").gameObject;
		child.SetActive(false);
		yield return new WaitForSeconds(sec);
		child.SetActive(true);
	}
}
