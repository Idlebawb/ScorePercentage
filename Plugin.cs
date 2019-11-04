using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using Harmony;


namespace ScorePercentage
{
    public class Plugin : IBeatSaberPlugin
    {
        public static string PluginName => "ScorePercentage";
        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;

        internal static HarmonyInstance harmony;

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            configProvider = cfgProvider;

            config = cfgProvider.MakeLink<PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig)
                    p.Store(v.Value = new PluginConfig() { RegenerateConfig = false });
                config = v;
            });
        }

        public void OnApplicationStart()
        {
            Logger.log.Debug("Starting ScorePercentage Plugin");
            Settings.Config.LoadConfig();
            harmony = HarmonyInstance.Create("com.Idlebob.BeatSaber.ScorePercentage");
            //Patch Classes
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public void OnApplicationQuit()
        {
            Logger.log.Debug("Stopping ScorePercentage Plugin");
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
