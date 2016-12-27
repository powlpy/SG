using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseUI : MonoBehaviour {

    Button firstButton;
    public Sprite noVolume;
    public Sprite midVolume;
    public Sprite fullVolume;
    public GameObject volumeHandle;
    Image volumeHandleImage;

    void Start() {
        firstButton = GetComponentInChildren<Button>();
        StartCoroutine(SelectFirstButton());
        volumeHandleImage = volumeHandle.GetComponent<Image>();
    }

    IEnumerator SelectFirstButton() {
        EventSystem.current.SetSelectedGameObject(null, new BaseEventData(EventSystem.current));
        yield return new WaitForSeconds(0.1f);
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>().SetVolume(mySlider.value / 3.0f);
        UpdateVolumeSprite(mySlider);
    }

    void UpdateVolumeSprite(Slider mySlider) {
        if (mySlider.value == 0)
            volumeHandleImage.sprite = noVolume;
        else if (mySlider.value < 4)
            volumeHandleImage.sprite = midVolume;
        else
            volumeHandleImage.sprite = fullVolume;
    }

}
