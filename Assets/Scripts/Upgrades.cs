using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Upgrades : MonoBehaviour {

    static Upgrades Instance;
    public GameObject buttons;
    Image myImage;
    CharacterInventoryModel inventory;
    int[] prices;
    UpgradeButtonBehavior selectedButton;

    void Awake() {
        
        myImage = GetComponent<Image>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInventoryModel>();
        prices = new int[] { 10, 20, 30};
        Instance = this;
    }

    void Start() {
        UpdatePrices();
    }

    public static void Show() {
        Instance.InstanceShow();
    }

    void InstanceShow() {
        myImage.enabled = true;
        buttons.SetActive(true);
        buttons.GetComponentInChildren<Button>().Select();


    }

    public static void Hide() {
        Instance.InstanceHide();
    }

    public void InstanceHide() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>().setFrozen(false, true);
        StartCoroutine(HideElements());

    }

    public static bool IsShowing() {
        return Instance.InstanceIsShowing();
    }

    IEnumerator HideElements() {
        yield return new WaitForSeconds(0);
        myImage.enabled = false;
        buttons.SetActive(false);
    }

    bool InstanceIsShowing() {
        return myImage.enabled;
    }

    public void ButtonPressed(int i) {
        if (inventory.GetItemCount(ItemType.RecyclingPoints) < prices[i]) return;
        inventory.AddItem(ItemType.RecyclingPoints, -prices[i]);
        prices[i] += 2;
        UpdatePrices();
        InstanceButtonSelected(selectedButton);

        switch (selectedButton.index) {
            case 0:
                Debug.Log("Augmenter les pv");
                break;
            case 1:
                Debug.Log("Augmenter la vitesse");
                break;
            case 2:
                Debug.Log("Augmenter les dégats");
                break;
            default:
                Debug.Log("Not implemented");
                break;
        }

    }

    public static void ButtonSelected(UpgradeButtonBehavior behavior) {
        Instance.InstanceButtonSelected(behavior);
    }

    void InstanceButtonSelected(UpgradeButtonBehavior behavior) {
        selectedButton = behavior;
        if (prices[behavior.index] > inventory.GetItemCount(ItemType.RecyclingPoints))
            behavior.SetColor(Color.red);
        else
            behavior.SetColor(Color.green);
    }

    void UpdatePrices() {
        int nbUpgrades = transform.GetChild(0).childCount - 1;
        for (int i = 0; i < nbUpgrades; i++) {
            transform.GetChild(0).GetChild(i).Find("Cost").GetComponentInChildren<Text>().text = prices[i].ToString("000");
        }
    }

}
