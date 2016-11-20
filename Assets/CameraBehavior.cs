using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	

	void Update () {
        Vector3 vectMovement = transform.position;
        vectMovement.x = player.transform.position.x;
        transform.position = vectMovement;
	}
}
