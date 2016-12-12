using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour {

    public GameObject panelOptions;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void StartNewGame()
    {
        SceneManager.LoadScene("myProject");
    }

    public void ShowOptions()
    {
        panelOptions.SetActive(!panelOptions.activeInHierarchy);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game exited ! Bye bye !");
    }
}
