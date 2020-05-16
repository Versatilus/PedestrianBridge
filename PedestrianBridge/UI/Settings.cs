using ColossalFramework.UI;
using ICities;
using ColossalFramework;

namespace PedestrianBridge.UI {
    using Tool;
    public static class ModSettings {
        public const string FileName = nameof(PedestrianBridge);
        static ModSettings() {
            // Creating setting file - from SamsamTS
            if (GameSettings.FindSettingsFileByName(FileName) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = FileName } });
            }
        }

        public static void OnSettingsUI(UIHelperBase helper) {
            UIHelper group = helper.AddGroup("Pedestrian Bridge") as UIHelper;
            UIPanel panel = group.self as UIPanel;
            var keymappings = panel.gameObject.AddComponent<KeymappingsPanel>();
            keymappings.AddKeymapping("Activation Shortcut", PedBridgeTool.ActivationShortcut);
        }
    }
}
