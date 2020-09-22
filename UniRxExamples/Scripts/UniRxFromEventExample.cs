using System;
using UniRx;
using UnityEngine;

namespace UniRxExamples.Scripts
{
    public class UniRxFromEventExample : MonoBehaviour
    {
        private event Action<int> MockEvent;

        private void Start()
        {
            Observable.FromEvent<int>(
                    h => MockEvent += h,
                    h => MockEvent -= h)
                .TakeUntilDestroy(this)
                .Subscribe(count => print("Trigger num:" + count));

            MockEvent?.Invoke(1);
            MockEvent?.Invoke(100);
        }
    }
}