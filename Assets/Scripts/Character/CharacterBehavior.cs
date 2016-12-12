using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

    public GameObject PlayerHpHandler;
    public float moveSpeed = 6;
    public Transform weaponParent;
    public Transform shieldParent;
    public Transform previewItemParent;
    private bool isMoving;
    Animator anim;
    private static float movementX;
    private static float movementY;
    public Vector3 vectMovement = new Vector3(0,0,0);
    Rigidbody2D myBody;
    private CharacterInteractionModel InteractionModel;
    bool isFrozen = false;
    bool isAttacking = false;
    public ItemType equipedWeapon = ItemType.None;
    private float pushTime;
    private Vector3 pushDirection;
    private InteractablePickup carriedObject = null;
    private string layerOfCarriedObject;
    private float maxHp = 3;
    private float currentHp;
    private bool isImmune = false;


    void Start() {
        currentHp = maxHp;
        UpdateDisplayHearts();
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        InteractionModel = GetComponent<CharacterInteractionModel>();
        HideWeapon();
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
        else if (QuestionBox.IsVisible()) {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                QuestionBox.LowerSelection();

            if (Input.GetKeyDown(KeyCode.UpArrow))
                QuestionBox.GoUpSelection();
        }
    }

    void UpdateMovement() {

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
        if (isAttacking) return false;  //already attacking
        if (equipedWeapon == ItemType.None) return false;   //no weapon equiped
        if (IsBeingPushed()) return false;  //being pushed
        if (IsCarrying()) return false;     //carrying smth
        return true;
    }

    public void EquipWeapon(ItemType weapon) {
        if (weaponParent == null) return;
        ItemData itemData = Database.Items.FindItem(weapon);
        if (itemData == null) return;
        if (itemData.Equip == ItemData.EquipType.NotEquipable) return;

        equipedWeapon = weapon;

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
    }

    public void OnAttackStarted() {
        isAttacking = true;
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
    }

    public void StartPickUp1Animation() {
        anim.SetTrigger("DoPickUp1");
    }
    
    public void StartPickUp2Animation() {
        anim.SetTrigger("DoPickUp2");
    }

    //character is pushed back in a direction
    public void PushBack(Vector3 pushVect, float time) {
        if (isImmune) return;
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

    public void LoseHp(float i) {
        if (isImmune) return;
        StartCoroutine(ManageImmunity(1.5f));
        currentHp -= i;
        UpdateDisplayHearts();
		if (currentHp <= 0) {
			Debug.Log ("dead");
			EndGame ();
		}
		
    }
						//  Game Over
	[SerializeField]
	private GameObject gameOverUI;

	public void EndGame(){
		Debug.Log ("GAME OVER");
		gameOverUI.SetActive (true);
	}
		

    IEnumerator ManageImmunity(float delay) {
        StartCoroutine(BlinkRenderer(delay));
        isImmune = true;
        yield return new WaitForSeconds(delay);
        isImmune = false;
    }

    IEnumerator BlinkRenderer(float duration) {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        duration -= 0.3f;
        if (duration >= 0.15)
            StartCoroutine(BlinkRenderer(duration));

    }

    void UpdateDisplayHearts() {
        PlayerHpHandler.GetComponent<PlayerHpBehavior>().UpdateDisplay(maxHp, currentHp);
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
        EnemyBehavior objectBehavior = carriedObject.GetComponent<EnemyBehavior>();
        if (objectBehavior != null)
            objectBehavior.DisableShadow();
        TipsHandler.Recycle();
    }

    public void DropObject() {
        if (!IsCarrying()) return;

		// Vérification qu'il n'y a pas d'obstacle devant
		Collider2D[] closeColliders = Physics2D.OverlapCircleAll(transform.position + (vectMovement * 0.7f), 0.55f);
		foreach (Collider2D closeCollider in closeColliders) {
			if (closeCollider.CompareTag ("Obstacle")) {
				// L'objet est déposé derrière
				vectMovement = -vectMovement;
				break;
			}
		}

        anim.SetTrigger("DoDrop");  //drop animation
        Vector3 newPosition = vectMovement;
        newPosition.y -= previewItemParent.transform.localPosition.y + 0.2f;
        carriedObject.transform.localPosition = newPosition;    //move it in front of character
        carriedObject.transform.parent = null;  //no parent
        carriedObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = layerOfCarriedObject;
        if(carriedObject.GetComponentInChildren<SpriteRenderer>().sortingOrder < 200)
            carriedObject.GetComponentInChildren<SpriteRenderer>().sortingOrder += 100;
        SetColliders(carriedObject.gameObject, true); EnemyBehavior objectBehavior = carriedObject.GetComponent<EnemyBehavior>();
        if (objectBehavior != null)
            objectBehavior.EnableShadow();
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

	public void GainPoints(int nb) {
        CharacterInventoryModel inventory = GetComponent<CharacterInventoryModel>();
        if (inventory == null) return;
        inventory.AddItem(ItemType.RecyclingPoints, nb);
        TipsHandler.RecyclingPoints();
    }

	public void LosePoints(int nb) {

    }

}