using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

    public float moveSpeed = 2;
    public float pushStrength;
    public float pushTime;
    private bool isMoving;
    Animator anim;
    private static float movementX;
    private static float movementY;
    private Vector3 vectMovement = new Vector3(0, 0, 0);
    Rigidbody2D myBody;
    public bool isAwake;
    public GameObject player;
    private float myPushTime;
    private Vector3 pushDirection;

    void Start() {

        anim = GetComponentInChildren<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        isAwake = false;
    }
    
    void Update() {
        if (IsBeingPushed()) {
            myBody.velocity = pushDirection;
            Debug.Log(myPushTime);
            myPushTime = Mathf.MoveTowards(myPushTime, 0f, Time.deltaTime);
            return;
        }
        if (!isAwake) return;
        FollowPlayer();

    }

    public void Awaken() {
        isAwake = true;
        anim.SetBool("IsMoving", true);
    }

    public void Sleep() {
        isAwake = false;
        anim.SetBool("IsMoving", false);
    }

    public void OnHit(Collider2D weaponCollider) {
        PushBack(weaponCollider);
        Sleep();
    }

    void FollowPlayer() {
        Vector3 vectMovement = player.transform.position - transform.position;
        vectMovement.x += Random.Range(-1, 1);
        vectMovement.y += Random.Range(-1, 1);
        vectMovement.Normalize();
        myBody.velocity = vectMovement * moveSpeed;
    }

    public void OnHitCharacter() {
        Vector3 pushDirection = player.transform.position - transform.position;
        pushDirection.Normalize();
        player.GetComponent<CharacterBehavior>().PushBack(pushDirection * pushStrength, pushTime);
        Sleep();
    }

    public void PushBack(Collider2D weapon) {
        pushDirection = transform.position - weapon.transform.position;
        pushDirection.Normalize();
        pushDirection *= pushStrength;
        myPushTime = pushTime/10f;
    }

    public bool IsBeingPushed() {
        return (myPushTime > 0f);
    }

}
