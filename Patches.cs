using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;
using SecretHistories.Manifestations;
using HarmonyLib;

[HarmonyPatch(typeof(CardManifestation), nameof(CardManifestation.HideDecayView))]
public class MainPatch : Patch
{
    public MainPatch() {
        this.original = AccessTools.Method(typeof(CardManifestation), "HideDecayView");
        this.patch = AccessTools.Method(typeof(MainPatch), "Transpiler");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        codes.Insert(0, new CodeInstruction(OpCodes.Ret));
        return codes.AsEnumerable();
    }

}

