using UnityEngine;
using System.Collections;

public class InteractableRandomSign : Interactable {

    public string myText;

    public override void OnInteract(Character character) {
        if (DialogBox.IsVisible()) {
            DialogBox.Hide();
            character.Behavior.setFrozen(false, true);
        } else {
            myText = Database.Information.GetRandomStatement();
            DialogBox.Show(myText);
            character.Behavior.setFrozen(true, true);
        }
    }


}
