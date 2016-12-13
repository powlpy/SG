using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	// Use this for initialization
	public void Quit()
	{
		Debug.Log ("Application Quit !");
		//Application.Quit ();
		SceneManager.LoadScene("menu");

	}

	public void Retry ()
	{
		
		Debug.Log ("Application Retry !");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
