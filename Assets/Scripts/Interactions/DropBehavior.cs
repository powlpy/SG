using UnityEngine;
using System.Collections;

public class DropBehavior : MonoBehaviour {

    public TrashType TypeOfTrash;
    public GameObject RecyclingEffect;
    public GameObject WrongEffect;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            GameObject carriedObject = collider.GetComponent<CharacterBehavior>().GetCarriedObject();
            if(carriedObject != null) {
                collider.GetComponent<CharacterBehavior>().DropObject();
                if (carriedObject.GetComponent<InteractablePickup>().TypeOfTrash == TypeOfTrash) {
                    Destroy(carriedObject);
					collider.GetComponent<CharacterBehavior>().GainPoints(carriedObject.GetComponent<EnemyBehavior> ().score);
                    if (RecyclingEffect != null) {
                        GameObject myEffect = (GameObject)Instantiate(RecyclingEffect);
                        myEffect.transform.position = transform.position;
                    }
                }else {
                    EnemyBehavior behavior = carriedObject.GetComponent<EnemyBehavior>();
                    if (behavior != null) behavior.MakeStronger();
                    //collider.GetComponent<CharacterBehavior>().LosePoints();
                    if(WrongEffect != null) {
                        GameObject myEffect = (GameObject)Instantiate(WrongEffect);
                        myEffect.transform.position = transform.position;

                    }
                }
            }
        }
    }

}
