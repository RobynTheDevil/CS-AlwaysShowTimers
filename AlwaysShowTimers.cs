using System;
using System.Collections.Generic;
using UnityEngine;
using SecretHistories;
using HarmonyLib;

public class AlwaysShowTimers : MonoBehaviour
{
    public static PatchTracker showTimers {get; private set;}

    public void Start() {
        try
        {
            AlwaysShowTimers.showTimers = new PatchTracker("AlwaysShowTimers", new MainPatch());
        }
        catch (Exception ex)
        {
          NoonUtility.LogException(ex);
        }
        NoonUtility.Log("AlwaysShowTimers: Trackers Started");
    }

    public static void Initialise() {
        //Harmony.DEBUG = true;
        Patch.harmony = new Harmony("robynthedevil.alwaysshowtimers");
		new GameObject().AddComponent<AlwaysShowTimers>();
        NoonUtility.Log("AlwaysShowTimers: Initialised");
	}

}

