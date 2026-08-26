using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace TNRD.Zeepkist.ObjectProperties.Patches;

[HarmonyPatch(typeof(LEV_ScalingPanel), nameof(LEV_ScalingPanel.Awake))]
public class LEV_ScalingPanel_Awake
{
    [UsedImplicitly]
    public static void Postfix(LEV_ScalingPanel __instance)
    {
        Object.Destroy(__instance.gameObject);
    }
}
