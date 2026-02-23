using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace WarningMarker.Configuration
{
    [UsedImplicitly]
    public class ConfigFileData
    {
        public static ConfigFileData Instance { get; set; }

        [UsedImplicitly]
        public string ConfigVersion = ConfigDefaults.ConfigVersion;

        public bool ShowResets = ConfigDefaults.ShowResets;
        public bool EnableSimpleDetection = ConfigDefaults.EnableSimpleDetection;
        public bool EnableAdvancedDetection = ConfigDefaults.EnableAdvancedDetection;
        public bool HmdOnly = ConfigDefaults.HmdOnly;

        public bool ShowHorizontals = ConfigDefaults.ShowHorizontals;

        public bool ShowAllNotes = ConfigDefaults.ShowAllNotes;

        public float MarkerSize = ConfigDefaults.MarkerSize;

        public float MarkerOffset = ConfigDefaults.MarkerOffset;

        public bool ShowTopLayer = ConfigDefaults.ShowTopLayer;
        public bool ShowMiddleLayer = ConfigDefaults.ShowMiddleLayer;
        public bool ShowBottomLayer = ConfigDefaults.ShowBottomLayer;
    }
}