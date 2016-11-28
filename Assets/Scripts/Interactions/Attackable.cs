using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour {

    public virtual void OnHit(ItemType item) {

        Debug.Log("No OnHit event for item " + item);

    }
}
