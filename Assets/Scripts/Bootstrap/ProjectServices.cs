using AdvancedRPG.Audio;
using AdvancedRPG.Core;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Save;
using AdvancedRPG.Services;
using UnityEngine;

namespace AdvancedRPG.Bootstrap
{
    public static class ProjectServices
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetServices()
        {
            ProjectBootstrapper.ResetInstance();
            ServiceLocator.Clear();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterDefaultServices()
        {
            RegisterDefaults();
        }

        public static void RegisterDefaults(AudioSource musicSource = null, AudioClip victoryClip = null, bool overwrite = false)
        {
            RegisterIfNeeded<IDamageService>(new DamageService(), overwrite);
            RegisterIfNeeded<ISceneLoader>(new UnitySceneLoader(), overwrite);
            RegisterIfNeeded<IAudioService>(new UnityAudioService(musicSource, victoryClip), overwrite);
            RegisterIfNeeded<ISaveRepository>(new JsonFileSaveRepository("save.json"), overwrite);
            RegisterIfNeeded(new SaveLoadInteractor(ServiceLocator.Get<ISaveRepository>()), overwrite);
        }

        private static void RegisterIfNeeded<T>(T service, bool overwrite) where T : class
        {
            if (overwrite || !ServiceLocator.TryGet<T>(out _))
                ServiceLocator.Register(service);
        }
    }
}
