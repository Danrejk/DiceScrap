using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using LethalLib.Modules;
using UnityEngine;
using NetworkPrefabs = LethalLib.Modules.NetworkPrefabs;

namespace DiceScrap
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle MyCustomAssets;
        public Item piosikDie;

        private void Awake()
        {
            InitPiosikDie();
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void InitPiosikDie()
        {
            var assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "landmine_placer");
            var assetBundle = AssetBundle.LoadFromFile(assetDir);

            MyCustomAssets = AssetBundle.LoadFromFile(Path.Combine(assetDir, "piosikdiedata"));
            if (MyCustomAssets == null)
            {
                Logger.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
                return;
            }

            var rarity = 500; //30
            piosikDie = assetBundle.LoadAsset<Item>("Assets/piosikdiedata.asset");
            LethalLib.Modules.Utilities.FixMixerGroups(piosikDie.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(piosikDie.spawnPrefab);
            LethalLib.Modules.Items.RegisterScrap(piosikDie, rarity, LethalLib.Modules.Levels.LevelTypes.All);

        }
    }
}
