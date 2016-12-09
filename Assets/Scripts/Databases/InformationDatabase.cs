using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InformationDatabase : ScriptableObject {

    public List<Information> info = new List<Information>();
    public int GetNbStatements() {
        return info.Count;
    }
    public string GetStatement(int i) {
        return info[i].statement;
    }
    public string GetRandomStatement() {
        int i = Random.Range(0, info.Count);
        return info[i].statement;
    }

}

[System.Serializable]
public class Information {
    public string statement;
    public string question;
    public string answer1;
    public string answer2;
    public string answer3;
    public bool seen = false;
}
