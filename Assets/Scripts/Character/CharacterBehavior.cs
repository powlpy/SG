using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

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


    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        InteractionModel = GetComponent<CharacterInteractionModel>();
        HideWeapon();
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

        if (IsBeingPushed()) {
            myBody.velocity = pushDirection;
            pushTime = Mathf.MoveTowards(pushTime, 0f, Time.deltaTime);
            anim.SetBool("IsHit", IsBeingPushed());
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

    public void setFrozen(bool frozen, bool freezeTime) {
        isFrozen = frozen;
        if (freezeTime) {
            if (isFrozen)
                StartCoroutine(FreezeTimeRoutine());
            else
                Time.timeScale = 1;
        }
    }

    IEnumerator FreezeTimeRoutine() {
        yield return null;
        Time.timeScale = 0;
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
        if (isAttacking) return false;
        if(equipedWeapon == ItemType.None) return false;
        if (IsBeingPushed()) return false;
        return true;
    }
    
    public void EquipWeapon(ItemType weapon) {
        if (weaponParent == null) return;
        ItemData itemData = Database.Items.FindItem(weapon);
        if (itemData == null) return;
        if (itemData.Equip == ItemData.EquipType.NotEquipable) return;

        equipedWeapon = weapon;

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
        if (weaponParent == null) return;
        weaponParent.gameObject.SetActive(true);
    }

    public void HideWeapon() {
        if (weaponParent == null) return;
        weaponParent.gameObject.SetActive(false);
    }

    public void HidePreviewItem() {
        foreach (Transform child in previewItemParent.transform)
            GameObject.Destroy(child.gameObject);
    }

    public void SetWeaponBehind() {
        if (weaponParent == null) return;
        weaponParent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 90;
    }

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

    public void PushBack(Vector3 pushVect, float time) {
        pushTime = time;
        pushDirection = pushVect;
        OnAttackFinished();
        setFrozen(false, false);
        HidePreviewItem();
        HideWeapon();
        anim.SetBool("IsHit", true);
    }

    public bool IsBeingPushed() {
        return (pushTime > 0);
    }

    IEnumerator Sleep(float time) {
        yield return new WaitForSeconds(time);
    }

}