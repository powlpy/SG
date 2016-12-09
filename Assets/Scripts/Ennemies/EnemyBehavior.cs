using UnityEngine;
using System.Collections;

public enum BehaviorType {Follower, Wanders, WandersFollower, Immobile};

public class EnemyBehavior : MonoBehaviour {

    public float maxHealth;
	public float currentHealth;
    public float moveSpeed = 2;
	public bool isAngry = false;
    public float pushStrength;
    public float pushTime;
    public float stunTime;
    private float myStunTime;
    public float deathAnimationDelay;
    public GameObject deathAnimation;
    public float immunityWindow;
    private bool isMoving;
    Animator anim;
    private static float movementX;
    private static float movementY;
    Rigidbody2D myBody;
    public bool isAwake;
    private GameObject player;
    private float myPushTime;
    private Vector3 pushDirection;
	private Vector3 wandersDirection;
	private Vector3 precPosition;
    private bool isFrozen = false;
    private bool isImmune = false;
    public GameObject myShadow;
    public GameObject stunnedShadow;

	public BehaviorType behavior;
	public int score = 3;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
        myBody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        currentHealth = maxHealth;
        isAwake = false;
        GetComponentInParent<InteractablePickup>().SetAuthorizePickup(false);
    }
    
    void Update() {
        if (IsBeingCarried()) return;
        if(IsStunned()) {
            myStunTime = Mathf.MoveTowards(myStunTime, 0f, Time.deltaTime);
            if (!IsStunned()) {
                StopStun();
            }
        }
        if (IsBeingPushed()) {
            myBody.velocity = pushDirection;
            myPushTime = Mathf.MoveTowards(myPushTime, 0f, Time.deltaTime);
            if (!IsBeingPushed()) myBody.velocity = Vector3.zero;
            return;
        }
        if (isFrozen) return;
        if (!isAwake) return;

		switch (behavior) {
		case BehaviorType.Follower:
			FollowPlayer ();
			break;
		case BehaviorType.Wanders:
			RandomMove ();
			break;
		case BehaviorType.WandersFollower:
			FollowPlayer ();
			RandomMove ();
			break;
		default:
			break;
		}
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
        if (isImmune) return;
        StartCoroutine(ManageImmunity(immunityWindow));
        currentHealth--;
        CheckHealth();
        PushBack(weaponCollider);
    }

    void CheckHealth() {
        if(currentHealth <= 0 && !IsStunned()) {
            StunEnnemy();
        }
    }

    void StunEnnemy() {
        GetComponentInParent<InteractablePickup>().SetAuthorizePickup(true);
        myStunTime = stunTime;
        anim.SetBool("IsStunned", true);
        SetFrozen(true);
        if(stunnedShadow != null) {
            myShadow.SetActive(false);
            stunnedShadow.SetActive(true);
        }
    }

    void StopStun() {
		myStunTime = 0f;
        currentHealth = maxHealth;
        anim.SetBool("IsStunned", false);
        SetFrozen(false);
        GetComponentInParent<InteractablePickup>().SetAuthorizePickup(false);
        if (stunnedShadow != null) {
            myShadow.SetActive(true);
            stunnedShadow.SetActive(false);
        }
    }

    public bool IsStunned() {
        return (myStunTime > 0f);
    }

    void OnDeath() {
        Destroy(gameObject);
    }

    void FollowPlayer() {
        Vector3 vectMovement = player.transform.position - transform.position;
        vectMovement.x += Random.Range(-1, 1);
        vectMovement.y += Random.Range(-1, 1);
        vectMovement.Normalize();
        myBody.velocity = vectMovement * moveSpeed;
    }

	void RandomMove() {
		if (Random.Range (0, 12) == 1 || transform.position == precPosition) {
			int rdir = Random.Range (0, 4);
			switch (rdir) {
			case 1:
				wandersDirection.x = 1;
				wandersDirection.y = 0;
				break;
			case 2:
				wandersDirection.x = 0;
				wandersDirection.y = 1;
				break;
			case 3:
				wandersDirection.x = -1;
				wandersDirection.y = 0;
				break;
			default:
				wandersDirection.x = 0;
				wandersDirection.y = -1;
				break;
			}
		}

		precPosition = transform.position;
		myBody.velocity = wandersDirection * moveSpeed;
	}

    public void OnHitCharacter() {
        Vector3 pushDirection = player.transform.position - transform.position;
        pushDirection.Normalize();
        player.GetComponent<CharacterBehavior>().PushBack(pushDirection * pushStrength, pushTime);
        player.GetComponent<CharacterBehavior>().LoseHp(0.5f);
	}

    public void PushBack(Collider2D weapon) {
        pushDirection = transform.position - weapon.transform.position;
        pushDirection.Normalize();
        pushDirection *= pushStrength;
        myPushTime = pushTime;
    }

    public bool IsBeingPushed() {
        return (myPushTime > 0f);
    }

    public void SetFrozen(bool frozen) {
        isFrozen = frozen;
    }

    IEnumerator ManageImmunity(float delay) {
        isImmune = true;
        yield return new WaitForSeconds(delay);
        isImmune = false;
    }

    public bool IsBeingCarried() {
        return (gameObject.transform.parent != null);
    }

    public void MakeStronger() {
		if(score > 0)
			score--;
		if (!isAngry) {
			isAngry = true;
			moveSpeed *= 1.4f;
			currentHealth = maxHealth;
		}
        StopStun();

    }

    public void EnableShadow() {
        if (myShadow == null) return;
        if(stunnedShadow == null)
            myShadow.SetActive(true);
        else
            stunnedShadow.SetActive(true);

    }

    public void DisableShadow() {
        if (myShadow == null) return;
        if (stunnedShadow == null)
            myShadow.SetActive(false);
        else
            stunnedShadow.SetActive(false);


    }

}
