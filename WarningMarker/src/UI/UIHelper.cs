using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Settings;

namespace WarningMarker.UI
{
    public static class UIHelper
    {
        private const string GameplaySetupResource = Plugin.ResourcesPath + ".BSML.GameplaySetupUI.bsml";
        private const string SettingsResource = Plugin.ResourcesPath + ".BSML.SettingsUI.bsml";
        private const string TabName = Plugin.FancyName;

        public static void RegisterGameplaySetupTab()
        {
            GameplaySetup.Instance.AddTab(
                TabName,
                GameplaySetupResource,
                GameplaySetupUI.Instance
            );
        }

        public static void RemoveGameplaySetupTab()
        {
            GameplaySetup.Instance.RemoveTab(TabName);
        }

        public static void RegisterModSettingsMenu()
        {
            BSMLSettings.Instance.AddSettingsMenu(
                TabName,
                SettingsResource,
                SettingsUI.Instance
            );
        }
    }
}