using System;
using HarmonyLib;
using JetBrains.Annotations;

namespace TNRD.Zeepkist.ObjectProperties.Patches;

[HarmonyPatch(typeof(LEV_InspectorGUICreator), nameof(LEV_InspectorGUICreator.CreateGUI))]
public class LEV_InspectorGUICreator_CreateGUI
{
    public static event Action<LEV_InspectorGUICreator> OnPostfix;
        
    [UsedImplicitly]
    public static void Postfix(LEV_InspectorGUICreator __instance)
    {
        OnPostfix?.Invoke(__instance);
    }
}
