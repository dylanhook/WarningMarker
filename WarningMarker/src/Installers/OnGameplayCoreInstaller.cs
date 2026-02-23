using JetBrains.Annotations;
using WarningMarker.Configuration;
using WarningMarker.Managers;
using WarningMarker.Parity;
using Zenject;

namespace WarningMarker.Installers
{
    [UsedImplicitly]
    public class OnGameplayCoreInstaller : Installer<OnGameplayCoreInstaller>
    {
        public override void InstallBindings()
        {
            if (!PluginConfig.Enabled) return;

            var markerPrefab = new UnityEngine.GameObject("WarningMarkerPrefab");
            markerPrefab.AddComponent<WarningMarker>();
            markerPrefab.SetActive(false);

            Container.BindMemoryPool<WarningMarker, WarningMarker.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(markerPrefab)
                .UnderTransformGroup("WarningMarkers");

            Container.BindInterfacesAndSelfTo<WarningManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<HmdLayerManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ParityAnalysisManager>().AsSingle();
        }
    }
}