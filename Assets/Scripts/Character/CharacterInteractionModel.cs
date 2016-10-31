using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( CharacterBehavior))]

public class CharacterInteractionModel : MonoBehaviour {
    
    private Character character;

    void Awake() {
        character = GetComponent<Character>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnInteract() {

        Interactable usableInteractable = FindUsableInteractable();

        if(usableInteractable == null) {
            return;
        }

        usableInteractable.OnInteract(character);

    }

    Interactable FindUsableInteractable() {

        Collider2D[] closeColliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
        Interactable closestInteractable = null;
        float angleToClosestInteractable = Mathf.Infinity;

        foreach (Collider2D closeCollider in closeColliders) {

            Interactable colliderInteractable = closeCollider.GetComponent<Interactable>();
            if(colliderInteractable == null) {
                continue;
            }

            Vector3 directionToInteractable = closeCollider.transform.position - transform.position;

            float angleToInteractable = Vector3.Angle(character.Behavior.GetDirection(), directionToInteractable);
            
            if(angleToInteractable < 50) {
                if(angleToInteractable < angleToClosestInteractable) {
                    angleToClosestInteractable = angleToInteractable;
                    closestInteractable = colliderInteractable;
                }
            }
        }

        return closestInteractable;
    }

}
