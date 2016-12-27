﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehavior : MonoBehaviour {

    //FMOD.Studio.EventInstance myInstance;
    //FMOD.Studio.ParameterInstance myParameter;

    FMODUnity.StudioEventEmitter emitter;

    void Start() {
		/*
        myInstance = FMODUnity.RuntimeManager.CreateInstance("event:/mainMusic");
        myInstance.getParameter("intensity", out myParameter);//*/
		emitter = GetComponent<FMODUnity.StudioEventEmitter>(); 

    }


    public void goFast() {
		/*
        float temp;
        myParameter.getValue(out temp);
        myParameter.setValue(0.8f);
        myParameter.getValue(out temp);//*/
		Debug.Log("G2G FAST!");
		emitter.SetParameter ("intensity", 0.8f);
    }

    public void goSlow() {
		/*
        float temp;
        myParameter.getValue(out temp);
        myParameter.setValue(0.2f);
        myParameter.getValue(out temp);//*/
		Debug.Log("G2G SLOW!");
		emitter.SetParameter ("intensity", 0.2f);
    }

	public void inPause() {
		Debug.Log ("EN PAUSE");
		emitter.SetParameter ("paused", 1f);
	}

	public void outPause() {
		Debug.Log ("JEU REPRIS");
		emitter.SetParameter ("paused", 0f);
	}
}
