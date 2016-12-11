using UnityEngine;
using System.Collections;

public class Description : Interactable {

	public string myText;

	public override void OnInteract(Character character) {
		if (DescriptionBox.IsVisible()) {
			DescriptionBox.Hide();
			character.Behavior.setFrozen(false, true);
		} else {
			//myText = Database.Information.GetRandomStatement();
			//myText  = ("this is test");
			DescriptionBox.Show(myText);
			character.Behavior.setFrozen(true, true);
		}
	}


}