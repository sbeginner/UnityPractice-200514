using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniRxExamples.Scripts
{
    public class UniRxTwoInputFieldSummaryExample : MonoBehaviour
    {
        [SerializeField] private InputField field1 = default;
        [SerializeField] private InputField field2 = default;
        [SerializeField] private Text text = default;

        private void Start()
        {
            Observable.CombineLatest(field1.OnValueChangedAsObservable().Where(s => s != ""),
                    field2.OnValueChangedAsObservable().Where(s => s != ""))
                .TakeUntilDestroy(this)
                .Throttle(TimeSpan.FromSeconds(.3f))
                .Select(list => list.Sum(double.Parse))
                .OnErrorRetry((FormatException ex) =>
                {
                    text.text = "Error";
                    Debug.Log(ex);
                })
                .SubscribeToText(text);
        }
    }
}