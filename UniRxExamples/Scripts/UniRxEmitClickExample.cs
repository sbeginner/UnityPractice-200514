using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UniRxExamples.Scripts
{
    public class UniRxEmitClickExample : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle= default;
        [SerializeField] private Image image = default;

        private void Start()
        {
            image.OnPointerUpAsObservable()
                .Timestamp()
                .WithLatestFrom(
                    image.OnPointerDownAsObservable().Timestamp(),
                    (l, r) => new {up = l, down = r}
                )
                .Select(x => x.up.Timestamp - x.down.Timestamp)
                .Do(_ => particle.Emit(10))
                .Subscribe(x => Debug.Log(x));
        }
    }
}