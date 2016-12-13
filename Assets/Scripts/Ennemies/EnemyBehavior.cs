using UnityEngine;
using System.Collections;

public enum BehaviorType {Follower, Wanders, WandersFollower, ExitedWanders, ExitedWandersFollower, Immobile};

public class EnemyBehavior : MonoBehaviour {

    public float maxHealth;
	public float currentHealth;
    public float moveSpeed = 2;
	public bool isAngry;
    public float pushStrength;
    public float pushTime;
    public float stunTime;
    private float myStunTime;
    Object spawnAnimation;
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

    private bool isInArena = false;

    private bool lookingLeft = true;

	public bool isLast = false;

    public BehaviorType behavior;
	public int score = 3;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        isAngry = false;
        spawnAnimation = Resources.Load("EnnemySpawn");
    }

    void Start() {
        currentHealth = maxHealth;
        isAwake = true;
        GetComponentInParent<InteractablePickup>().SetAuthorizePickup(false);
        Awaken();
        GetComponent<Rigidbody2D>().isKinematic = true;
        StartCoroutine(SetInArenaAfterDelay(1));
        GameObject animation = (GameObject)Instantiate(spawnAnimation);
        animation.transform.position = transform.position;
        player.GetComponent<CharacterBehavior>().AddEnemy();
		Camera.main.GetComponent<CameraBehavior> ().nbEnnemies++;
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

        if (player.transform.position.x > transform.position.x && lookingLeft) {
            transform.Rotate(new Vector3(0, 180, 0));
            lookingLeft = false;
        } else if (player.transform.position.x < transform.position.x && !lookingLeft) {
            transform.Rotate(new Vector3(0, -180, 0));
            lookingLeft = true;
        }

            if (!isInArena) {
            FollowPlayer();
            return;
        }
		switch (behavior) {
		case BehaviorType.Follower:
			FollowPlayer ();
			break;
		case BehaviorType.Wanders:
			RandomMove (12);
			break;
		case BehaviorType.WandersFollower:
			FollowPlayer (); // À modifié, la vélocité est écrasé
			RandomMove (12); 
			break;
		case BehaviorType.ExitedWanders:
			RandomMove (3);
			break;
		case BehaviorType.ExitedWandersFollower:
			FollowPlayer (); // À modifié, la vélocité est écrasé
			RandomMove (3); 
			break;
		default:
			break;
		}
    }

    public void Awaken() {
        isAwake = true;
        anim.SetBool("IsMoving", true);
        TipsHandler.Ennemies();
    }

    public void Sleep() {
        if (isAngry) return;
        isAwake = false;
        anim.SetBool("IsMoving", false);
    }

    public void OnHit(Collider2D weaponCollider) {
        if (isImmune) return;
        StartCoroutine(ManageImmunity(immunityWindow));
        currentHealth -= player.GetComponent<CharacterBehavior>().damage;
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
        TipsHandler.PickUp();
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

    public void OnDeath() {
		CameraBehavior cam = Camera.main.GetComponent<CameraBehavior> ();
		cam.nbEnnemies--;
		if (isLast)
			cam.lastEnnemieIsDestroy = true;
		if(cam.nbEnnemies <= 0 && cam.lastEnnemieIsDestroy)
           cam.isFrozen = false;
        Destroy(gameObject);
    }

    void FollowPlayer() {
        Vector3 vectMovement = player.transform.position - transform.position;
        vectMovement.x += Random.Range(-1, 1);
        vectMovement.y += Random.Range(-1, 1);
        vectMovement.Normalize();
        myBody.velocity = vectMovement * moveSpeed;
    }

	void RandomMove(int excitation) {
		if (Random.Range (0, excitation) == 1 || transform.position == precPosition) {
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
        player.GetComponent<CharacterBehavior>().AddError();
        if (score > 0)
			score--;
		if (!isAngry) {
			isAngry = true;
			moveSpeed *= 1.4f;
        }
        currentHealth = maxHealth;
        StartCoroutine(WakeUpAfterDelay(1));
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

    IEnumerator WakeUpAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        StopStun();
    }

    IEnumerator SetInArenaAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        isInArena = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

	public void SetLast(bool b){
		isLast = b;
	}
}
