using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterInventoryModel : MonoBehaviour {

    Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();
    private CharacterBehavior Behavior;

    void Awake() {
        Behavior = GetComponent<CharacterBehavior>();
    }
    
    void Start() {

    }

    public void AddItem(ItemType itemType) {
        AddItem(itemType, 1);

    }

    public void AddItem(ItemType itemType, int amount) {
        if (items.ContainsKey(itemType)) {
            items[itemType] += amount;
        } else {
            items.Add(itemType, amount);
        }

        if(itemType == ItemType.Sword && amount > 0) {
            Behavior.EquipWeapon(itemType);
            Behavior.StartPickUp1Animation();
        }

    }
   
}
