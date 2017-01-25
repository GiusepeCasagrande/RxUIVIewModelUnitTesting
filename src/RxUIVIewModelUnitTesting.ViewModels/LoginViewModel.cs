using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using RxUIVIewModelUnitTesting.Services;
using Splat;
using Xamvvm;

namespace RxUIVIewModelUnitTesting.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        string m_userName;
        public string UserName
        {
            get
            {
                return m_userName;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref m_userName, value);
            }
        }

        string m_password;
        public string Password
        {
            get
            {
                return m_password;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref m_password, value);
            }
        }

        public ReactiveCommand<Unit, string> LoginCommand
        {
            get;
            set;
        }

        public LoginViewModel() : this(null)
        {
        }

        IAuthenticationSystem m_authenticationSystem;

        public LoginViewModel(IAuthenticationSystem authenticationSystem = null, IScheduler scheduler = null) : base(scheduler)
        {
            m_authenticationSystem = authenticationSystem ?? Locator.Current.GetService<IAuthenticationSystem>();

            var canLogin =
                this.WhenAny(
                    x => x.UserName,
                    x => x.Password,
                    (username, password) => (!string.IsNullOrWhiteSpace(username.Value) && !string.IsNullOrWhiteSpace(password.Value)));

            LoginCommand = ReactiveCommand.CreateFromTask(async _ => await m_authenticationSystem.LoginAsync(UserName, Password), canLogin, m_scheduler);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                LoginCommand
                    .ThrownExceptions
                    .SubscribeOn(m_scheduler)
                    .Subscribe(ex =>
                        {
                            var text = (ex.GetType() == typeof(InvalidUserException)) ? "Invalid User Or Password" : "";
                            ShowGenericError(disposables, text);
                        })
                    .DisposeWith(disposables);

                LoginCommand
                    .Subscribe(token => this.PushPageFromCacheAsync<SomeViewModel>())
                    .DisposeWith(disposables);

                LoginCommand
                    .IsExecuting
                    .Throttle(TimeSpan.FromSeconds(0.1))
                    .SubscribeOn(m_scheduler)
                    .Subscribe((isRunning) => IsRunning = isRunning)
                    .DisposeWith(disposables);
            });
        }
    }
}

