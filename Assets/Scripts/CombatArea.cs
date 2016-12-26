using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatArea : MonoBehaviour {

    public int nbEnnemies;
    public float delaiBetweenEnnemies = 5;

    public bool greyEnnemies;
    public bool yellowEnnemies;
    public bool glassEnnemies;
    public bool orangeEnnemies;

    List<Object> ennemiesList;

    bool saveFollowAxisX;
    bool saveFollowAxisY;
    bool hasTriggered = false;
    float cameraWidth;
    float cameraHeight;

    AudioClip exclamationSound;

    GameObject player;

    void Awake() {
        ennemiesList = new List<Object>();
        if(greyEnnemies)
            ennemiesList.AddRange(Resources.LoadAll("Ennemies/Grey"));
        if (yellowEnnemies)
            ennemiesList.AddRange(Resources.LoadAll("Ennemies/Yellow"));
        if (glassEnnemies)
            ennemiesList.AddRange(Resources.LoadAll("Ennemies/Glass"));
        if (orangeEnnemies)
            ennemiesList.AddRange(Resources.LoadAll("Ennemies/Orange"));
        if (ennemiesList.Count == 0)
            Debug.Log("Erreur : aucun type d'ennemis dans la zone " + name);

        exclamationSound = (AudioClip)Resources.Load("Sounds/exclamation");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start() {
        cameraWidth = Camera.main.GetComponent<CameraBehavior>().GetWidth();
        cameraHeight = Camera.main.GetComponent<CameraBehavior>().GetHeight();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag != "Player") return;
        if (hasTriggered) return;
        hasTriggered = true;

        collider.transform.Find("exclamation").GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(DisableExclamation());
        //collider.GetComponent<CharacterBehavior>().PlaySound(exclamationSound);
		CameraBehavior cam = Camera.main.GetComponent<CameraBehavior> ();
		cam.Frozen(transform.position.x, transform.position.y);
		cam.lastEnnemieIsDestroy = false;


        for (int i = 0; i < nbEnnemies-1; i++)
            StartCoroutine(SummonEnnemyAfterDelay(i * delaiBetweenEnnemies));
        StartCoroutine(SummonLastEnnemyAfterDelay((nbEnnemies - 1) * delaiBetweenEnnemies));
    }

    IEnumerator DisableExclamation() {
        yield return new WaitForSeconds(1.5f);
        GameObject.FindGameObjectWithTag("Player").transform.Find("exclamation").GetComponent<SpriteRenderer>().enabled = false;

    }

    void SummonEnnemy() {
        Object randomEnnemy = ennemiesList[Random.Range(0, ennemiesList.Count)];
        GameObject ennemy = Instantiate(randomEnnemy) as GameObject;
        
        ennemy.transform.position = GetRandomEnnemyPosition();
    }

    IEnumerator SummonEnnemyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        SummonEnnemy();
    }

    void SummonLastEnnemy() {
        Object randomEnnemy = ennemiesList[Random.Range(0, ennemiesList.Count)];
        GameObject ennemy = Instantiate(randomEnnemy) as GameObject;

        ennemy.transform.position = GetRandomEnnemyPosition();
        ennemy.GetComponent<EnemyBehavior>().SetLast(true);
    }

    IEnumerator SummonLastEnnemyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        SummonLastEnnemy();
    }

    Vector3 GetRandomEnnemyPosition() {
        /*
        Vector3 result = Camera.main.transform.position;

        Vector3 variation = new Vector3(1.1f * cameraWidth, Random.Range(-cameraHeight, cameraHeight), 0);
        if (Random.value < 0.5)
            result -= variation;
        else
            result += variation;

        result.z = 0;*/

        bool isValid = false;
        Vector3 result = Vector3.zero;
        int ii = 0;
        while (!isValid) {
            ii++;
            isValid = true;
            result = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0.1f * Screen.width, 0.9f * Screen.width), Random.Range(0.1f * Screen.height, 0.9f * Screen.height), Camera.main.farClipPlane / 2));
            result.z = 0;
            if (Physics2D.OverlapCircleAll(result, 0.8f).Length > 0)
                isValid = false;
            if (Vector3.Distance(player.transform.position, result) < 3)
                isValid = false;
        }
        return result;
    }

}
