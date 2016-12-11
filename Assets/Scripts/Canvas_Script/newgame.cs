using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class newgame : MonoBehaviour {
	public Button Button;

	void Start () {
		Button btn = Button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);



		//Application.LoadLevel(levelToLoad);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		//Application.LoadLevel("myProject");
	}
}