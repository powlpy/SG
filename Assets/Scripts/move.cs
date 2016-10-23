using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

    public float moveSpeed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate(new Vector3(-moveSpeed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow)){
            transform.Translate(new Vector3(0, moveSpeed, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            transform.Translate(new Vector3(0, -moveSpeed, 0) * Time.deltaTime);
        }

    }
}
