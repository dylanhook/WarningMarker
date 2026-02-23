using IPA;
using IPA.Config;
using IPA.Config.Stores;
using JetBrains.Annotations;
using WarningMarker.Configuration;
using WarningMarker.HarmonyPatches;
using WarningMarker.UI;
using IPALogger = IPA.Logging.Logger;

namespace WarningMarker
{
    [Plugin(RuntimeOptions.SingleStartInit), UsedImplicitly]
    public class Plugin
    {
        #region Constants

        internal const string ResourcesPath = "WarningMarker.src.Resources";
        internal const string HarmonyId = "dylan.WarningMarker";
        internal const string FancyName = "Warning Marker";

        #endregion

        #region Init

        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, Config config)
        {
            Log = logger;
            InitializeConfig(config);
        }

        private static void InitializeConfig(Config config)
        {
            ConfigFileData.Instance = config.Generated<ConfigFileData>();
        }

        #endregion

        #region OnApplicationStart

        [OnStart, UsedImplicitly]
        public void OnApplicationStart()
        {
            HarmonyHelper.ApplyPatches();
            BeatSaberMarkupLanguage.Util.MainMenuAwaiter.MainMenuInitializing += OnMainMenuInitializing;
        }

        private void OnMainMenuInitializing()
        {
            UIHelper.RegisterGameplaySetupTab();
            UIHelper.RegisterModSettingsMenu();
        }

        #endregion

        #region OnApplicationQuit

        [OnExit, UsedImplicitly]
        public void OnApplicationQuit() { }

        #endregion
    }
}