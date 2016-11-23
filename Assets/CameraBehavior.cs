using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject player;
    public bool FollowAxisX = true;
    public bool FollowAxisY = true;	

	void Update () {
        Vector3 vectMovement = transform.position;
        if (FollowAxisX) {
            vectMovement.x = player.transform.position.x;
        }
        if (FollowAxisY) {
            vectMovement.y = player.transform.position.y;
        } else {
            vectMovement.y = 0f;
        }
        transform.position = vectMovement;
	}
}
