using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniRxExamples.Scripts
{
    public class UniRxSerializationExample : MonoBehaviour
    {
        public ViewModel Model => model;

        [SerializeField] private ViewModel model = default;
        [SerializeField] private View view = default;

        private void Start()
        {
            model.Hp
                .ObserveEveryValueChanged(ex => ex.Value < 100)
                .TakeUntilDestroy(this)
                .SubscribeToInteractable(view.AddHpButton);
            model.Hp
                .ObserveEveryValueChanged(ex => ex.Value > 0)
                .TakeUntilDestroy(this)
                .SubscribeToInteractable(view.SubHpButton);
            model.Hp
                .TakeUntilDestroy(this)
                .SubscribeToText(view.HpText);

            view.AddHpButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(onNext: _ => model.Hp.Value += 5);
            view.SubHpButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ => model.Hp.Value -= 5);
            view.SerializeJsonButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    var serializeJsonData = JsonUtility.ToJson(model);
                    Debug.LogFormat("Serialize Json: {0}", serializeJsonData);

                    var deserializeJsonData = JsonUtility.FromJson<ViewModel>(serializeJsonData);
                    Debug.LogFormat("Deserialize Json: {0}", deserializeJsonData.Hp.Value.ToString());
                });
        }


        [Serializable]
        public class ViewModel
        {
            public ReactiveProperty<int> Hp => hp;

            [SerializeField] private IntReactiveProperty hp;
            private CompositeDisposable Disposable { get; } = new CompositeDisposable();

            public ViewModel(IntReactiveProperty hp)
            {
                this.hp = hp.AddTo(Disposable);
            }

            public void Dispose()
            {
                Disposable.Dispose();
            }
        }


        [Serializable]
        public class View
        {
            public Button AddHpButton => addHpButton;
            public Button SubHpButton => subHpButton;
            public Button SerializeJsonButton => serializeJsonButton;
            public Text HpText => hpText;

            [SerializeField] private Button addHpButton = default;
            [SerializeField] private Button subHpButton = default;
            [SerializeField] private Button serializeJsonButton = default;
            [SerializeField] private Text hpText = default;
        }
    }
}