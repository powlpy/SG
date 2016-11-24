using UnityEngine;
using System.Collections;

public class DropBehavior : MonoBehaviour {

    public GameObject RecyclingEffect;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            GameObject carriedObject = collider.GetComponent<CharacterBehavior>().GetCarriedObject();
            if(carriedObject != null) {
                collider.GetComponent<CharacterBehavior>().DropObject();
                Destroy(carriedObject);
                if (RecyclingEffect != null) {
                    GameObject myEffect = (GameObject)Instantiate(RecyclingEffect);
                    myEffect.transform.position = transform.position;
                }

            }
        }
    }

}
