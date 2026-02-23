using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WarningMarker.Configuration;

namespace WarningMarker.UI
{
    public class SettingsUI : INotifyPropertyChanged
    {
        public static SettingsUI Instance { get; private set; } = new SettingsUI();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [UIValue("enable-simple-detection")]
        public bool EnableSimpleDetection
        {
            get => PluginConfig.EnableSimpleDetection;
            set
            {
                PluginConfig.EnableSimpleDetection = value;
                OnPropertyChanged();
            }
        }

        [UIValue("enable-advanced-detection")]
        public bool EnableAdvancedDetection
        {
            get => PluginConfig.EnableAdvancedDetection;
            set
            {
                PluginConfig.EnableAdvancedDetection = value;
                OnPropertyChanged();
            }
        }

        [UIValue("enable-hmd-only")]
        public bool EnableHmdOnly
        {
            get => PluginConfig.HmdOnly;
            set
            {
                PluginConfig.HmdOnly = value;
                OnPropertyChanged();
            }
        }

        [UIAction("reset-settings"), UsedImplicitly]
        private void ResetSettings()
        {
            EnableSimpleDetection = ConfigDefaults.EnableSimpleDetection;
            EnableAdvancedDetection = ConfigDefaults.EnableAdvancedDetection;
            EnableHmdOnly = ConfigDefaults.HmdOnly;

            GameplaySetupUI.Instance.ResetToDefaults();
        }
    }
}