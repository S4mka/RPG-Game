using AdvancedRPG.Core;
using AdvancedRPG.Services;
using UnityEngine;

namespace AdvancedRPG.UI
{
    public sealed class RestartButton : MonoBehaviour
    {
        public void Restart() { Time.timeScale = 1f; ServiceLocator.Get<ISceneLoader>().ReloadCurrent(); }
    }
}
