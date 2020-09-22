using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniRxExamples.Scripts
{
    public class UniRxTextIntervalOutputExample : MonoBehaviour
    {
        private const string CommentText = "This is a long long sentence.";
        [SerializeField] private Text dialogue = default;

        private void Start()
        {
            Observable.Interval(TimeSpan.FromMilliseconds(250))
                .Take(CommentText.Length)
                .Select(_ => 1)
                .Scan((accumulation, newValue) => accumulation + newValue)
                .DoOnCompleted(() => Debug.Log("complete"))
                .SubscribeToText(dialogue, length => CommentText.Substring(0, length))
                .AddTo(this);
        }
    }
}