using UnityEngine;
using System.Collections;

public class DropBehavior : MonoBehaviour {

    public TrashType TypeOfTrash;
    public GameObject RecyclingEffect;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            GameObject carriedObject = collider.GetComponent<CharacterBehavior>().GetCarriedObject();
            if(carriedObject != null) {
                collider.GetComponent<CharacterBehavior>().DropObject();
                if (carriedObject.GetComponent<InteractablePickup>().TypeOfTrash == TypeOfTrash) {
                    Destroy(carriedObject);
                    if (RecyclingEffect != null) {
                        GameObject myEffect = (GameObject)Instantiate(RecyclingEffect);
                        myEffect.transform.position = transform.position;
                    }
                }else {
                    EnemyBehavior behavior = carriedObject.GetComponent<EnemyBehavior>();
                    if (behavior != null) behavior.MakeStronger();
                }
            }
        }
    }

}
