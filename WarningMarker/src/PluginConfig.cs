namespace WarningMarker.Configuration
{
    public static class PluginConfig
    {
        public static bool Enabled => ShowResets || ShowHorizontals || ShowAllNotes;

        public static bool ShowResets
        {
            get => ConfigFileData.Instance.ShowResets;
            set => ConfigFileData.Instance.ShowResets = value;
        }

        public static bool EnableSimpleDetection
        {
            get => ConfigFileData.Instance.EnableSimpleDetection;
            set => ConfigFileData.Instance.EnableSimpleDetection = value;
        }

        public static bool EnableAdvancedDetection
        {
            get => ConfigFileData.Instance.EnableAdvancedDetection;
            set => ConfigFileData.Instance.EnableAdvancedDetection = value;
        }

        public static bool HmdOnly
        {
            get => ConfigFileData.Instance.HmdOnly;
            set => ConfigFileData.Instance.HmdOnly = value;
        }

        public static bool ShowHorizontals
        {
            get => ConfigFileData.Instance.ShowHorizontals;
            set => ConfigFileData.Instance.ShowHorizontals = value;
        }

        public static bool ShowAllNotes
        {
            get => ConfigFileData.Instance.ShowAllNotes;
            set => ConfigFileData.Instance.ShowAllNotes = value;
        }

        public static float MarkerSize
        {
            get => ConfigFileData.Instance.MarkerSize;
            set => ConfigFileData.Instance.MarkerSize = value;
        }

        public static float MarkerOffset
        {
            get => ConfigFileData.Instance.MarkerOffset;
            set => ConfigFileData.Instance.MarkerOffset = value;
        }

        public static bool ShowTopLayer
        {
            get => ConfigFileData.Instance.ShowTopLayer;
            set => ConfigFileData.Instance.ShowTopLayer = value;
        }

        public static bool ShowMiddleLayer
        {
            get => ConfigFileData.Instance.ShowMiddleLayer;
            set => ConfigFileData.Instance.ShowMiddleLayer = value;
        }

        public static bool ShowBottomLayer
        {
            get => ConfigFileData.Instance.ShowBottomLayer;
            set => ConfigFileData.Instance.ShowBottomLayer = value;
        }
    }
}