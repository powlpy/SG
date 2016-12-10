using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject player;
    private bool FollowAxisX = true;
    private bool FollowAxisY = false;
    private float minX, maxX, minY, maxY;
    private Vector3 vectMovement;
    public bool isFrozen = false;

    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject BottomWall;
    public GameObject UpperWall;

    float vertExtent, horzExtent;

    void Awake() {
        minX = -200;
        maxX = 200;
        minY = -200;
        maxY = 200;

        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        LeftWall.transform.localPosition = new Vector3(-horzExtent, 0, 0);
        RightWall.transform.localPosition = new Vector3(horzExtent, 0, 0);
        BottomWall.transform.localPosition = new Vector3(0, vertExtent, 0);
        UpperWall.transform.localPosition = new Vector3(0, -vertExtent, 0);

    }

	void LateUpdate () {
        if (isFrozen) return;
        vectMovement = transform.position;
        if (FollowAxisX) {
            vectMovement.x = player.transform.position.x;
            vectMovement.x = Mathf.Clamp(vectMovement.x, minX + horzExtent, maxX - horzExtent);
        } else {
            vectMovement.x = (minX + maxX) / 2f;
        }
        if (FollowAxisY) {
            vectMovement.y = player.transform.position.y;
            vectMovement.y = Mathf.Clamp(vectMovement.y, minY + vertExtent, maxY - vertExtent);
        } else {
            vectMovement.y = 0f;
        }
        transform.position = vectMovement;
	}
    
    public void SetFollowAxisX(bool b) {
        FollowAxisX = b;
    }

    public void SetFollowAxisY(bool b) {
        FollowAxisY = b;
    }

    public bool GetFollowAxisX() {
        return FollowAxisX;
    }

    public bool GetFollowAxisY() {
        return FollowAxisY;
    }

    public void SetMinX(float x) {
        minX = x;
    }

    public void SetMaxX(float x) {
        maxX = x;
    }
    public void SetMinY(float y) {
        minY = y;
    }
    public void SetMaxY(float y) {
        maxY = y;
    }

    public float GetWidth() {
        return horzExtent;
    }

    public float GetHeight() {
        return vertExtent;
    }

}
