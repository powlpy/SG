using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	// Use this for initialization
	public void Quit()
	{
		Debug.Log ("Application Quit !");
		Application.Quit ();
	}

	public void Retry ()
	{
		//Application.LoadLevel (Application.loadedLevel);
		Debug.Log ("Application Retry !");
		//SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		Application.LoadLevel("myProject");
	}

}
