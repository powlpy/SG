using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

    public float moveSpeed = 6;
    public Transform weaponParent;
    private bool isMoving;
    Animator anim;
    private static float movementX;
    private static float movementY;
    public Vector3 vectMovement = new Vector3(0,0,0);
    Rigidbody2D myBody;
    private CharacterInteractionModel InteractionModel;
    bool isFrozen = false;
    bool isAttacking = false;
    ItemType equipedWeapon = ItemType.None;
    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        InteractionModel = GetComponent<CharacterInteractionModel>();
        weaponParent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        UpdateAttack();
        UpdateAction();
        UpdateMovement();
    }

    void UpdateAttack() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (CanAttack()) {
                anim.SetTrigger("DoAttack");
            }
        }
    }

    void UpdateAction() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnActionPressed();
        }
    }

    void UpdateMovement() { 
        if(isFrozen || isAttacking) {
            myBody.velocity = new Vector3(0, 0, 0);
            return;
        }
        isMoving = false;
        movementX = 0;
        movementY = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            movementX = -1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            movementX = 1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            movementY = 1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            movementY = -1;
            isMoving = true;
        }
        if (isMoving) {
            vectMovement.x = movementX;
            vectMovement.y = movementY;
            vectMovement.Normalize();
            anim.SetFloat("MovementX", vectMovement.x);
            anim.SetFloat("MovementY", vectMovement.y);
            myBody.velocity = moveSpeed * vectMovement;
        } else {
            myBody.velocity = new Vector3(0, 0, 0);
        }
        anim.SetBool("IsMoving", isMoving && !isFrozen);

    }

    public void setFrozen(bool frozen) {
        isFrozen = frozen;
    }

    public Vector3 GetDirection() {
        return vectMovement;
    }

    void OnActionPressed() {
         if(InteractionModel == null) {
            return;
         }
        InteractionModel.OnInteract();
    }
    bool CanAttack() {
        if (isAttacking) {
            return false;
        }
        if(equipedWeapon == ItemType.None) {
            return false;
        }
        return true;
    }
    
    public void EquipWeapon(ItemType weapon) {
        equipedWeapon = weapon;
    }

    public void OnAttackStarted() {
        isAttacking = true;
        weaponParent.gameObject.SetActive(true);
    }

    public void OnAttackFinished() {
        Debug.Log("stop attack");
        weaponParent.gameObject.SetActive(false);
        isAttacking = false;
    }

    public void StartPickUp1Animation() {
        anim.SetTrigger("DoPickUp1");
    }

}