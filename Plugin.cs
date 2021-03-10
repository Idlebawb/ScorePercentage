using IPA;
using IPA.Config;
using IPA.Utilities;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using IPA.Logging;
using HarmonyLib;
using System;


namespace ScorePercentage
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static string PluginName => "ScorePercentage";
//        internal static Ref<PluginConfig> config;
        internal static Harmony harmony;
        internal static ScorePercentageCommon scorePercentageCommon;

        public static Logger log { get; private set; }

        [Init]
        public void Init(Logger logger, Config cfgProvider)
        {
            //Logger.log = logger;
            log = logger;
            PluginConfig.Instance = cfgProvider.Generated<PluginConfig>();

        }

        [OnStart]
        public void OnApplicationStart()
        {
            log.Debug("Starting ScorePercentage Plugin");
            //Settings.PluginConfig.LoadConfig();
            scorePercentageCommon = new ScorePercentageCommon();
            harmony = new Harmony("com.Idlebob.BeatSaber.ScorePercentage");
            //Patch Classes
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            log.Debug("Stopping ScorePercentage Plugin");
            harmony.UnpatchAll("com.Idlebob.BeatSaber.ScorePercentage");
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
