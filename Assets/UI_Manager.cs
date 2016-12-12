using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour {

    public GameObject panelOptions;
    public GameObject cloud1,cloud2;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        //--- Animation nuages---
        cloud1.transform.Translate(Vector3.left * 0.008f);
        cloud2.transform.Translate(Vector3.left * 0.005f);
        if(cloud1.transform.position.x < -12f)
        {
            cloud1.transform.position = new Vector3(12f, cloud1.transform.position.y, cloud2.transform.position.z);
        }
        if (cloud2.transform.position.x < -12f)
        {
            cloud2.transform.position = new Vector3(12f, cloud2.transform.position.y, cloud2.transform.position.z);
        }
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
