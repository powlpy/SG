using UnityEngine;
using System.Collections;

public class CombatArea : MonoBehaviour {

    public int nbEnnemies;

    Object[] ennemiesList;
    bool saveFollowAxisX;
    bool saveFollowAxisY;
    bool hasTriggered = false;
    float cameraWidth;
    float cameraHeight;

    void Awake() {
        ennemiesList = Resources.LoadAll("Ennemies");
    }

    void Start() {
        cameraWidth = Camera.main.GetComponent<CameraBehavior>().GetWidth();
        cameraHeight = Camera.main.GetComponent<CameraBehavior>().GetHeight();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag != "Player") return;
        if (hasTriggered) return;
        hasTriggered = true;
        Camera.main.GetComponent<CameraBehavior>().isFrozen = true;

        for (int i = 0; i < nbEnnemies-1; i++)
            StartCoroutine(SummonEnnemyAfterDelay(i * 7));
        StartCoroutine(SummonLastEnnemyAfterDelay((nbEnnemies - 1) * 7));
    }

    void SummonEnnemy() {
        Object randomEnnemy = ennemiesList[Random.Range(0, ennemiesList.Length)];
        GameObject ennemy = Instantiate(randomEnnemy) as GameObject;
        
        ennemy.transform.position = GetRandomEnnemyPosition();
    }

    IEnumerator SummonEnnemyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        SummonEnnemy();
    }

    void SummonLastEnnemy() {
        Object randomEnnemy = ennemiesList[Random.Range(0, ennemiesList.Length)];
        GameObject ennemy = Instantiate(randomEnnemy) as GameObject;

        ennemy.transform.position = GetRandomEnnemyPosition();
        ennemy.GetComponent<EnemyBehavior>().SetLast(true);
    }

    IEnumerator SummonLastEnnemyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        SummonLastEnnemy();
    }

    Vector3 GetRandomEnnemyPosition() {

        Vector3 result = Camera.main.transform.position;

        Vector3 variation = new Vector3(1.1f * cameraWidth, Random.Range(-cameraHeight, cameraHeight), 0);
        if (Random.value < 0.5)
            result -= variation;
        else
            result += variation;

        result.z = 0;

        return result;
    }

}
