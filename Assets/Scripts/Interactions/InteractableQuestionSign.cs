using UnityEngine;
using System.Collections;

public class InteractableQuestionSign : Interactable {

    string[] myText;

    void Start() {
        myText = Database.Information.GetRandomQuestion();
    }

    public override void OnInteract(Character character) {
        if (QuestionBox.IsVisible()) {
            if (QuestionBox.IsChecked())
                QuestionBox.Hide();
            else
                QuestionBox.CheckAnswer();
        } else {
            QuestionBox.ShowQuestion(myText);
            character.Behavior.setFrozen(true, true);
        }
    }


}
