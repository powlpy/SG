using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class HealthBar : MonoBehaviour {

	public GameObject healthBar;



	public void SetDamamges(float value){
		healthBar.GetComponentInParent<Scrollbar> ().size -= value;
		if (healthBar.GetComponentInParent<Scrollbar> ().size == 0 ){
			Debug.Log("Deceder");

			//Application.LoadLevel("menu");
		}

	}
}