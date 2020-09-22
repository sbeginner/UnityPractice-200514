using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniRxExamples.Scripts
{
    public class UniRxLoadSceneExample : MonoBehaviour
    {
        private void Start()
        {
            LoadSceneRx();
        }

        private void LoadSceneRx()
        {
            var progressObservable = new ScheduledNotifier<float>();
            progressObservable.Subscribe(progress => { Debug.LogFormat("Loading: {0}", progress); });

            // Load Default Scene 0
            SceneManager.LoadSceneAsync(0).AsAsyncOperationObservable(progressObservable)
                .Subscribe(asyncOperation =>
                {
                    var player = new GameObject {name = "[NewObject] Jack"};
                    Debug.Log($"Done! Hi, {player.name}.");
                });
        }
    }
}