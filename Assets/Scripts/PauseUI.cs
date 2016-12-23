using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour {

    Button firstButton;

    void OnEnable() {
        firstButton = GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(null, new BaseEventData(EventSystem.current));
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject, new BaseEventData(EventSystem.current));
    }

    // Use this for initialization
    public void Quit(){
		Debug.Log ("Application Quit !");
		//Application.Quit ();
		SceneManager.LoadScene("menu");

	}

	public void Retry (){
		
		Debug.Log ("Application Retry !");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    public void Resume() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>().ResumeGame();
    }

    public void ChangeVolume(GameObject myObject) {
        Slider mySlider = myObject.GetComponent<Slider>();
        GameObject.FindGameObjectWithTag("Global").GetComponent<AudioSource>().volume = 0.1f * mySlider.value / 3.0f;
    }

}
