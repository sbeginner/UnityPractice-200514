using System;
using UniRx;

namespace UniRxExamples.Concepts
{
    public static class ObservableUserAuth
    {
        public static IObservable<User> SignUpAsync(string mail, string id, string password)
        {
            return Observable.Create<User>(observer =>
            {
                var user = new User(mail, id, password);

                user.SignUpAsync(e =>
                {
                    if (e == null)
                    {
                        observer.OnNext(user);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(e);
                    }
                });
                return Disposable.Create(() => { });
            });
        }

        public static IObservable<User> LoginAsync(string id, string password)
        {
            return Observable.Create<User>(observer =>
            {
                var user = User.CurrentUser;

                User.LogInAsync(id, password, e =>
                {
                    if (e == null)
                    {
                        observer.OnNext(user);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(e);
                    }
                });
                return Disposable.Create(() => { });
            });
        }

        public static IObservable<Unit> LogoutAsync()
        {
            return Observable.Create<Unit>(observer =>
            {
                User.LogOutAsync(e =>
                {
                    if (e == null)
                    {
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(e);
                    }
                });
                return Disposable.Create(() => { });
            });
        }
    }

    public class User
    {
        public static User CurrentUser => _currentUser;
        private static User _currentUser;

        private string _email;
        private string _userName;
        private string _password;

        public User(string mail, string id, string password)
        {
            _email = mail;
            _userName = id;
            _password = password;
        }

        public static void LogInAsync(string id, string password, Action<Exception> action)
        {
            _currentUser = new User(null, id, password);
            // action(IsNotValidThrowException(_currentUser))
        }

        public void SignUpAsync(Action<Exception> action)
        {
            // action(IsNotValidThrowException(this))
        }

        public static void LogOutAsync(Action<Exception> action)
        {
            // action(IsNotValidThrowException(this))
        }

        public void SignUp(string id, string mail, string pass)
        {
            ObservableUserAuth
                .SignUpAsync(mail, id, pass)
                .Subscribe(user =>
                    {
                        // UserProfileProvider.SetUser(user)
                        // onSuccessLoginSubject.OnNext(Unit.Default)
                    },
                    ex =>
                    {
                        // failedReasonSubject.OnNext(ex)
                    });
        }
    }
}