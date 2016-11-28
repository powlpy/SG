using UnityEngine;
using System.Collections;

public class InteractableSign : Interactable {

    public string myText;

    public override void OnInteract(Character character) {
        if (DialogBox.IsVisible()) {
            Time.timeScale = 1;
            DialogBox.Hide();
            character.Behavior.setFrozen(false);
        } else {
            DialogBox.Show(myText);
            character.Behavior.setFrozen(true);
            StartCoroutine(FreezeTimeRoutine());
        }
    }

    IEnumerator FreezeTimeRoutine() {
        yield return null;
        Time.timeScale = 0;
    }

}
