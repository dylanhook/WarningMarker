using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WarningMarker.Configuration;

namespace WarningMarker.UI
{
    public class GameplaySetupUI : INotifyPropertyChanged
    {
        public static GameplaySetupUI Instance { get; private set; } = new GameplaySetupUI();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetToDefaults()
        {
            ShowResetsValue = ConfigDefaults.ShowResets;
            ShowHorizontalsValue = ConfigDefaults.ShowHorizontals;
            ShowAllNotesValue = ConfigDefaults.ShowAllNotes;
            MarkerSizeValue = ConfigDefaults.MarkerSize;
            MarkerOffsetValue = ConfigDefaults.MarkerOffset;
            ShowTopLayerValue = ConfigDefaults.ShowTopLayer;
            ShowMiddleLayerValue = ConfigDefaults.ShowMiddleLayer;
            ShowBottomLayerValue = ConfigDefaults.ShowBottomLayer;
        }

        [UIValue("show-resets-value"), UsedImplicitly]
        private bool ShowResetsValue
        {
            get => PluginConfig.ShowResets;
            set
            {
                PluginConfig.ShowResets = value;
                OnPropertyChanged();
            }
        }

        [UIValue("show-horizontals-value"), UsedImplicitly]
        private bool ShowHorizontalsValue
        {
            get => PluginConfig.ShowHorizontals;
            set
            {
                PluginConfig.ShowHorizontals = value;
                OnPropertyChanged();
            }
        }

        [UIValue("show-all-notes-value"), UsedImplicitly]
        private bool ShowAllNotesValue
        {
            get => PluginConfig.ShowAllNotes;
            set
            {
                PluginConfig.ShowAllNotes = value;
                OnPropertyChanged();
            }
        }

        [UIValue("marker-size-value"), UsedImplicitly]
        private float MarkerSizeValue
        {
            get => PluginConfig.MarkerSize;
            set
            {
                PluginConfig.MarkerSize = value;
                OnPropertyChanged();
            }
        }

        [UIAction("marker-size-formatter"), UsedImplicitly]
        private string MarkerSizeFormatter(float value) => $"{value:F2}";

        [UIValue("marker-offset-value"), UsedImplicitly]
        private float MarkerOffsetValue
        {
            get => PluginConfig.MarkerOffset;
            set
            {
                PluginConfig.MarkerOffset = value;
                OnPropertyChanged();
            }
        }

        [UIAction("marker-offset-formatter"), UsedImplicitly]
        private string MarkerOffsetFormatter(float value) => $"{value:F1}";

        [UIValue("show-top-layer-value"), UsedImplicitly]
        private bool ShowTopLayerValue
        {
            get => PluginConfig.ShowTopLayer;
            set
            {
                PluginConfig.ShowTopLayer = value;
                OnPropertyChanged();
            }
        }

        [UIValue("show-middle-layer-value"), UsedImplicitly]
        private bool ShowMiddleLayerValue
        {
            get => PluginConfig.ShowMiddleLayer;
            set
            {
                PluginConfig.ShowMiddleLayer = value;
                OnPropertyChanged();
            }
        }

        [UIValue("show-bottom-layer-value"), UsedImplicitly]
        private bool ShowBottomLayerValue
        {
            get => PluginConfig.ShowBottomLayer;
            set
            {
                PluginConfig.ShowBottomLayer = value;
                OnPropertyChanged();
            }
        }
    }
}