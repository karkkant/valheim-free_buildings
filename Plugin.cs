using BepInEx;
using HarmonyLib;

namespace FreeBuildings
{
    [BepInPlugin("org.bepinex.plugins.free-buildings", "Free Buildings", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new Harmony("org.bepinex.plugins.free-buildings");

        private void Awake()
        {
            _harmony.PatchAll();
        }
    }
}
