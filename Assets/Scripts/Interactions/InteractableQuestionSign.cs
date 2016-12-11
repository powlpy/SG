using UnityEngine;
using System.Collections;

public class InteractableQuestionSign : Interactable {

    public override void OnInteract(Character character) {
        if (QuestionBox.IsVisible()) {
            QuestionBox.CheckAnswer();
        } else {
            QuestionBox.ShowQuestion(Database.Information.GetRandomQuestion());
            character.Behavior.setFrozen(true, true);
        }
    }


}
