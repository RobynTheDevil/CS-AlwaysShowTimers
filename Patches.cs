using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;
using SecretHistories.Manifestations;
using HarmonyLib;

[HarmonyPatch(typeof(CardManifestation), nameof(CardManifestation.Initialise))]
public class MainPatch : Patch
{
    public MainPatch() {
        this.original = AccessTools.Method(typeof(CardManifestation), "Initialise");
        this.patch = AccessTools.Method(typeof(MainPatch), "Transpiler");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        int ind = PatchHelper.FindLdstrOperand(codes, "CardManifestation_");
        codes.Insert(ind - 2, new CodeInstruction(OpCodes.Ldarg_0)); //this
        codes.Insert(ind - 1, new CodeInstruction(OpCodes.Ldc_I4_1)); //true
        codes.Insert(ind, new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(CardManifestation), "_alwaysDisplayDecayView")));
        return codes.AsEnumerable();
    }

}

