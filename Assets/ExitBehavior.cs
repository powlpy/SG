using UnityEngine;
using System.Collections;

public class ExitBehavior : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag != "Player") return;
        Debug.Log((collider.GetComponent<CharacterBehavior>().GetScore() * 100).ToString() + " %");

    }


}
