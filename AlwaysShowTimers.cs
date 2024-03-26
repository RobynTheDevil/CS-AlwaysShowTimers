using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SecretHistories;
using SecretHistories.UI;
using SecretHistories.Manifestations;
using SecretHistories.Entities;
using SecretHistories.Spheres;
using SecretHistories.Abstract;
using HarmonyLib;

public class AlwaysShowTimers : MonoBehaviour
{
    public static PatchTracker showTimers {get; private set;}
    public static SettingTrackerUpdate whenUpdated = WhenSettingUpdated;

    public void Start() {
        try
        {
            AlwaysShowTimers.showTimers = new PatchTracker("AlwaysShowTimers", new MainPatch(), AlwaysShowTimers.whenUpdated);
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

    public static IEnumerable<ElementStack> GetCards() {
        return Watchman.Get<HornedAxe>().GetExteriorSpheres()
            .Where<Sphere>((Func<Sphere, bool>) (x => (double) x.TokenHeartbeatIntervalMultiplier > 0.0))
            .SelectMany<Sphere, Token>((Func<Sphere, IEnumerable<Token>>) (x => x.GetTokens()))
            .Select<Token, ITokenPayload>((Func<Token, ITokenPayload>) (x => x.Payload))
            .OfType<ElementStack>()
            .Where<ElementStack>((Func<ElementStack, bool>) (x => (x.Decays)));
    }

    public static void WhenSettingUpdated(object _) {
        if (AlwaysShowTimers.showTimers.current) {
            AlwaysShowTimers.Enable();
        } else {
            AlwaysShowTimers.Disable();
        }
    }

    public static void Enable() {
        IEnumerable<ElementStack> cards = AlwaysShowTimers.GetCards();
        foreach (ElementStack card in cards) {
            Traverse.Create(card).Field("_alwaysDisplayDecayView").SetValue(true);
            Traverse.Create(card).Field("_token").Field("_manifestation").Method("ShowDecayView").GetValue();
        }
    }

    public static void Disable() {
        IEnumerable<ElementStack> cards = AlwaysShowTimers.GetCards();
        foreach (ElementStack card in cards) {
            Traverse.Create(card).Field("_alwaysDisplayDecayView").SetValue(false);
            Traverse.Create(card).Field("_token").Field("_manifestation").Method("HideDecayView").GetValue();
        }
    }
}

