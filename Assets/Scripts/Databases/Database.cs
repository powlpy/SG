using UnityEngine;
using System.Collections;

public class Database {

    private static ItemsDatabase myItems;
    public static ItemsDatabase Items {
        get {
            if(myItems == null) {
                myItems = Resources.Load<ItemsDatabase>("Databases/ItemsDatabase"); 
            }
            return myItems;
        }
    }
    

    void TestMethod() {
        Debug.Log(Database.Items);
    }

}
