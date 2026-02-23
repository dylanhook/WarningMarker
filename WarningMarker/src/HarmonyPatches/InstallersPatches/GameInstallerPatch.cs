using System;
using HarmonyLib;
using JetBrains.Annotations;
using WarningMarker.Installers;
using Zenject;

namespace WarningMarker.HarmonyPatches.InstallersPatches
{
    [HarmonyPatch(typeof(GameplayCoreInstaller), "InstallBindings")]
    public static class GameInstallerPatch
    {
        [UsedImplicitly]

        private static void Postfix(GameplayCoreInstaller __instance)
        {
            try
            {
                var container = AccessTools.Property(typeof(GameplayCoreInstaller), "Container").GetValue(__instance, null) as DiContainer;
                OnGameplayCoreInstaller.Install(container);
            }
            catch (Exception ex)
            {
                Plugin.Log.Critical($"---\nGameInstaller exception: {ex.Message}\n{ex.StackTrace}\n---");
            }
        }
    }
}