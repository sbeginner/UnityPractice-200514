using UniRx;
using UnityEngine;

namespace UniRxExamples.Scripts
{
    public class UniRxCommandExample : MonoBehaviour
    {
        private void Start()
        {
            var where = new Subject<bool>();

            var rxComm = new ReactiveCommand<string>(where, false);
            rxComm.Subscribe(
                value => Debug.LogFormat("Next: {0}", value),
                ex => Debug.LogException(ex),
                () => Debug.LogFormat("Complete"));

            var execute = rxComm.CanExecute;
            var result = rxComm.Execute("hello rx command.");
            Debug.LogFormat("CanExecute: {0}, Execute: {1}", execute, result);

            where.OnNext(true);

            execute = rxComm.CanExecute;
            result = rxComm.Execute("hello rx command.");
            Debug.LogFormat("CanExecute: {0}, Execute: {1}", execute, result);

            where.OnNext(false);

            execute = rxComm.CanExecute;
            result = rxComm.Execute("hello rx command.");
            Debug.LogFormat("CanExecute: {0}, Execute: {1}", execute, result);
        }
    }
}