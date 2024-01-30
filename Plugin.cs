using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using LethalLib.Modules;
using UnityEngine;
using UnityEngine.Assertions;
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
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!!!!!!!!!!!!!!!!");
        }

        private void InitPiosikDie()
        {
            var assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "piosikdiedata");
            var assetBundle = AssetBundle.LoadFromFile(assetDir);

            piosikDie = assetBundle.LoadAsset<Item>("Assets/piosikdiedata.asset");
            if (piosikDie == null)
            {
                Logger.LogError("Could not load piosikdiedata item");
                return;
            }

            var piosikDiePrefab = piosikDie.spawnPrefab;
            if (piosikDiePrefab == null)
            {
                Logger.LogError("Could not load piosikdiedata prefab");
                return;
            }

            var physicsProp = piosikDiePrefab.GetComponent<PhysicsProp>();
            if (physicsProp == null)
            {
                Logger.LogError("PhysicsProp is missing on the piosikdiedata prefab.");
                return;
            }


            var rarity = 5000; //30
            LethalLib.Modules.Utilities.FixMixerGroups(piosikDie.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(piosikDie.spawnPrefab);
            LethalLib.Modules.Items.RegisterScrap(piosikDie, rarity, LethalLib.Modules.Levels.LevelTypes.All);

        }
    }
}
