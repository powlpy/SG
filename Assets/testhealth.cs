using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class testhealth : MonoBehaviour {

	public GameObject ennemie;
	public GameObject healthBar;
	private float maxHP = 0.1f;
	private float Reduc = 0.005f;
 

	void Start () {
		
	
	}


	void Update () {
		
		//ennemie = GameObject.FindGameObjectsWithTag("ennemie");

		if (Vector3.Distance(transform.position, ennemie.transform.position) <= 1)
		{
			healthBar.GetComponentInParent<Scrollbar> ().size -= Reduc;
			Debug.Log("touché par ennemie");
			if (healthBar.GetComponentInParent<Scrollbar> ().size == 0 ){
				Debug.Log("Deceder");

				//Application.LoadLevel("menu");
			}
		}
	
	}
}
