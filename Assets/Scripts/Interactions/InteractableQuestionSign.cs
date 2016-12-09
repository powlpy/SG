using UnityEngine;
using System.Collections;

public class InteractableQuestionSign : Interactable {

    public string myText;

    public override void OnInteract(Character character) {
        if (QuestionBox.IsVisible()) {
            QuestionBox.CheckAnswer();
        } else {
            myText = Database.Information.GetRandomStatement();
            QuestionBox.ShowQuestion(Database.Information.GetRandomQuestion());
            character.Behavior.setFrozen(true, true);
        }
    }


}
