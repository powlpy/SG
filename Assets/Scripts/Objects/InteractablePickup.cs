using UnityEngine;
using System.Collections;

public class InteractablePickup : Interactable {

    public override void OnInteract(Character character) {
        character.GetComponent<CharacterBehavior>().CarryObject(this);

        BroadcastMessage("OnPickupObject", character, SendMessageOptions.DontRequireReceiver);

    }


}
