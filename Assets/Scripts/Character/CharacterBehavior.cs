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
<<<<<<< HEAD
    ItemType equipedWeapon = ItemType.None;
    // Use this for initialization
=======
    public ItemType equipedWeapon = ItemType.None;
    private float pushTime;
    private Vector3 pushDirection;
    private InteractablePickup carriedObject = null;
    private string layerOfCarriedObject;
    
>>>>>>> origin/master
    void Start() {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        InteractionModel = GetComponent<CharacterInteractionModel>();
        weaponParent.gameObject.SetActive(false);
    }
    
    void Update() {
        UpdateAttack();
        UpdateAction();
        UpdateMovement();
    }

    void UpdateAttack() {
        if (Input.GetKeyDown(KeyCode.C))
            if (CanAttack())
                anim.SetTrigger("DoAttack");
    }

    void UpdateAction() {
        if (Input.GetKeyDown(KeyCode.Space))
            OnActionPressed();
    }

<<<<<<< HEAD
    void UpdateMovement() { 
        if(isFrozen || isAttacking) {
            myBody.velocity = new Vector3(0, 0, 0);
            return;
        }
=======
    void UpdateMovement() {

>>>>>>> origin/master
        isMoving = false;
        movementX = 0;
        movementY = 0;

        if (isFrozen || isAttacking) {
            myBody.velocity = new Vector3(0, 0, 0);
        } else if (IsBeingPushed()) {
            myBody.velocity = pushDirection;
            pushTime = Mathf.MoveTowards(pushTime, 0f, Time.deltaTime);
            anim.SetBool("IsHit", IsBeingPushed());
        } else {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                movementX = -1;
                isMoving = true;
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                movementX = 1;
                isMoving = true;
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                movementY = 1;
                isMoving = true;
            } else if (Input.GetKey(KeyCode.DownArrow)) {
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
        }
        anim.SetBool("IsMoving", isMoving && !isFrozen);
    }

<<<<<<< HEAD
    public void setFrozen(bool frozen) {
        isFrozen = frozen;
=======
    //freeze/unfreeze the character
    public void setFrozen(bool frozen, bool freezeTime) {
        isFrozen = frozen;
        if (freezeTime) {
            if (isFrozen)
                StartCoroutine(FreezeTimeRoutine());
            else
                Time.timeScale = 1;
        }
    }
    //wait end of frame before freezing the time for the animation
    IEnumerator FreezeTimeRoutine() {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0;
>>>>>>> origin/master
    }

    public Vector3 GetDirection() {
        return vectMovement;
    }

    //when player press interaction key
    void OnActionPressed() {
        if (IsCarrying()) {
            DropObject();
            return;
        }
        if(InteractionModel != null)
            InteractionModel.OnInteract();
    }

    //returns true if the character can attack
    bool CanAttack() {
<<<<<<< HEAD
        if (isAttacking) {
            return false;
        }
        if(equipedWeapon == ItemType.None) {
            return false;
        }
=======
        if (isAttacking) return false;  //already attacking
        if (equipedWeapon == ItemType.None) return false;   //no weapon equiped
        if (IsBeingPushed()) return false;  //being pushed
        if (IsCarrying()) return false;     //carrying smth
>>>>>>> origin/master
        return true;
    }
    
    public void EquipWeapon(ItemType weapon) {
        equipedWeapon = weapon;
<<<<<<< HEAD
=======

        //create the prefab of the weapon in WeaponParent
        GameObject newEquipableObject = (GameObject)Instantiate(itemData.Prefab);
        newEquipableObject.transform.parent = weaponParent;
        newEquipableObject.transform.localPosition = Vector2.zero;
        newEquipableObject.transform.localRotation = Quaternion.identity;
    }

    public void PreviewItem(ItemType item) {
        ItemData itemData = Database.Items.FindItem(item);
        if (itemData == null) return;
        if (itemData.Equip == ItemData.EquipType.NotEquipable) return;

        GameObject newEquipableObject = (GameObject)Instantiate(itemData.Prefab);
        newEquipableObject.transform.parent = previewItemParent;
        newEquipableObject.transform.localPosition = Vector2.zero;
        newEquipableObject.transform.localRotation = Quaternion.identity;

        if (itemData.Animation == ItemData.PickupAnimation.OneHanded)
            StartPickUp1Animation();
        else if (itemData.Animation == ItemData.PickupAnimation.TwoHanded)
            StartPickUp2Animation();
    }

    //player can't move when pickup animation is being played
    public void OnAnimationStarted() {
        setFrozen(true, false);
    }

    public void OnAnimationFinished() {
        setFrozen(false, false);
>>>>>>> origin/master
    }

    public void OnAttackStarted() {
        isAttacking = true;
<<<<<<< HEAD
        weaponParent.gameObject.SetActive(true);
    }

    public void OnAttackFinished() {
        Debug.Log("stop attack");
        weaponParent.gameObject.SetActive(false);
        isAttacking = false;
=======
    }

    public void OnAttackFinished() {
        isAttacking = false;
    }

    public void ShowWeapon() {
        if (weaponParent != null)
            weaponParent.gameObject.SetActive(true);
    }

    public void HideWeapon() {
        if (weaponParent != null)
            weaponParent.gameObject.SetActive(false);
    }

    public void HidePreviewItem() {
        foreach (Transform child in previewItemParent.transform)
            GameObject.Destroy(child.gameObject);
    }

    //draw weapon behind character
    public void SetWeaponBehind() {
        if (weaponParent == null) return;
        weaponParent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 90;
    }

    //draw weapon above character
    public void SetWeaponAbove() {
        if (weaponParent == null) return;
        weaponParent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 110;
>>>>>>> origin/master
    }

    public void StartPickUp1Animation() {
        anim.SetTrigger("DoPickUp1");
    }
<<<<<<< HEAD
=======
    
    public void StartPickUp2Animation() {
        anim.SetTrigger("DoPickUp2");
    }

    //character is pushed back in a direction
    public void PushBack(Vector3 pushVect, float time) {
        pushTime = time;
        pushDirection = pushVect;
        OnAttackFinished();         //end attack
        setFrozen(false, false);    //freeze player
        if (IsCarrying())
            DropObject();
        HidePreviewItem();
        HideWeapon();
        anim.SetBool("IsHit", true);
    }

    //returns true if player is being pushed right now
    public bool IsBeingPushed() {
        return (pushTime > 0);
    }

    //Pickup and carry an InteractablePickup item
    public void CarryObject(InteractablePickup objectToCarry) {
        if (IsCarrying()) return;
        carriedObject = objectToCarry;
        anim.SetTrigger("DoPickupObject");  //pickup animation
        anim.SetBool("IsPickingUp", true);  //special Idle and Walk animation when carrying
        carriedObject.transform.parent = previewItemParent; //object becomes child of previewItemParent
        carriedObject.transform.localPosition = Vector3.zero;
        layerOfCarriedObject = carriedObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
        carriedObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "HighObjects";    //rendered above the character
        SetColliders(carriedObject.gameObject, false);  //disable all coliders of the carried object
    }

    public void DropObject() {
        if (!IsCarrying()) return;
        anim.SetTrigger("DoDrop");  //drop animation
        Vector3 newPosition = vectMovement;
        newPosition.y -= previewItemParent.transform.localPosition.y + 0.2f;
        carriedObject.transform.localPosition = newPosition;    //move it in front of character
        carriedObject.transform.parent = null;  //no parent
        carriedObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = layerOfCarriedObject;
        if(carriedObject.GetComponentInChildren<SpriteRenderer>().sortingOrder < 200)
            carriedObject.GetComponentInChildren<SpriteRenderer>().sortingOrder += 100;
        SetColliders(carriedObject.gameObject, true);
        carriedObject = null;
        anim.SetBool("IsPickingUp", false);     //go back to normal animations
    }

    //true if player is carrying smth
    public bool IsCarrying() {
        return (carriedObject != null);
    }

    //enable/disable all colliders and rigidbody of an object and its children
    void SetColliders(GameObject obj, bool b) {
        if (obj.GetComponent<Rigidbody2D>() != null)
            obj.GetComponent<Rigidbody2D>().isKinematic = !b;

        if (obj.GetComponent<Collider2D>() != null)
            obj.GetComponent<Collider2D>().enabled = b;

        foreach(Collider2D collider in obj.GetComponentsInChildren<Collider2D>())
            collider.enabled = b;
    }

    //returns the carried object or null
    public GameObject GetCarriedObject() {
        if (carriedObject == null) return null;
        return carriedObject.gameObject;
    }
>>>>>>> origin/master

}