using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehavior : MonoBehaviour {

    FMOD.Studio.EventInstance myInstance;
    FMOD.Studio.ParameterInstance myParameter;

    FMOD_StudioEventEmitter emitter;

    void Start() {
        myInstance = FMODUnity.RuntimeManager.CreateInstance("event:/mainMusic");
        myInstance.getParameter("intensity", out myParameter);
    }


    public void goFast() {
        float temp;
        myParameter.getValue(out temp);
        Debug.Log("G2G FAST!");
        myParameter.setValue(0.8f);
        myParameter.getValue(out temp);
    }

    public void goSlow() {
        float temp;
        myParameter.getValue(out temp);
        Debug.Log("G2G SLOW!");
        myParameter.setValue(0.2f);
        myParameter.getValue(out temp);
    }

}
