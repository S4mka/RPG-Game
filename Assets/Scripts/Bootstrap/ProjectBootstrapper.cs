using AdvancedRPG.Audio;
using AdvancedRPG.Core;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Save;
using AdvancedRPG.Services;
using UnityEngine;

namespace AdvancedRPG.Bootstrap
{
    public sealed class ProjectBootstrapper : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip victoryClip;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Clear();
            ServiceLocator.Register<IDamageService>(new DamageService());
            ServiceLocator.Register<ISceneLoader>(new UnitySceneLoader());
            ServiceLocator.Register<IAudioService>(new UnityAudioService(musicSource, victoryClip));
            ServiceLocator.Register<ISaveRepository>(new JsonFileSaveRepository("save.json"));
            ServiceLocator.Register(new SaveLoadInteractor(ServiceLocator.Get<ISaveRepository>()));
        }
    }
}
