using FMODSyntax;
using HarmonyLib;
using JetBrains.Annotations;

namespace TNRD.Zeepkist.ObjectProperties.Patches;

[HarmonyPatch(typeof(LEV_InspectorGUICreator), nameof(LEV_InspectorGUICreator.ToggleAdvanced))]
public class LEV_InspectorGUICreator_ToggleAdvanced
{
    [UsedImplicitly]
    public static bool Prefix()
    {
        AudioEvents.Blarghl.Play(null);
        return false;
    }
}
