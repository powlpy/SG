using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour {

    public ItemType item;

    void OnTriggerEnter2D(Collider2D collider) {

        Attackable attackable = collider.gameObject.GetComponent<Attackable>();
        if(attackable != null) {
            attackable.OnHit(item);
        }

    }

}
