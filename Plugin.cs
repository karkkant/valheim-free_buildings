using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;

namespace FreeBuildings
{
    [BepInPlugin(pluginId, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        const string pluginId = "org.bepinex.plugins.free-buildings";
        const string pluginName = "Free Buildings";
        const string pluginVersion = "1.0.1";
        
        private readonly Harmony _harmony = new Harmony(pluginId);

        ConfigSync configSync = new ConfigSync(pluginId) { DisplayName = pluginName, CurrentVersion = pluginVersion, MinimumRequiredVersion = pluginVersion };
        private static ConfigEntry<bool> LockConfig;
        private static ConfigEntry<bool> UnlockAllBuildPieces;
        
        public static bool UnlockAllBuildPiecesEnabled => UnlockAllBuildPieces.Value;
        
        private void Awake()
        {
            LockConfig = config("General", "LockConfig", true, "If on, the configuration is locked and can be changed by server admins only. [Synced with server]");
            UnlockAllBuildPieces = config("General", "UnlockAllBuildPieces", false, "Unlock all build pieces without needing to discover recipes. Note: Toggling requires restarting game.");
            
            _harmony.PatchAll();
        }
        
        #region ServerSync

        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        #endregion
    }
}
