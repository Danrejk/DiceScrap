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
        public Item PiosikDice;

        private void Awake()
        {
            InitPiosikDie();
            // Plugin startup logic
            Logger.LogInfo("Dzień dobry, proszę o chwilę cierpliwości.");
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
            Logger.LogWarning("Proszę Państwa, mam w domu awarię wodną, dlatego sposób przekazania wyniku prac nad portfolio podam później.");
        }

        private void InitPiosikDie()
        {
            var assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "piosikdiedata");
            var assetBundle = AssetBundle.LoadFromFile(assetDir);

            PiosikDice = assetBundle.LoadAsset<Item>("Assets/piosikdiedata.asset");
            if (PiosikDice == null)
            {
                Logger.LogError("Could not load piosikdiedata item");
                return;
            }

            var piosikDiePrefab = PiosikDice.spawnPrefab;
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


            var rarity = 30; //30
            LethalLib.Modules.Utilities.FixMixerGroups(PiosikDice.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(PiosikDice.spawnPrefab);
            LethalLib.Modules.Items.RegisterScrap(PiosikDice, rarity, LethalLib.Modules.Levels.LevelTypes.All);

        }
    }
}
