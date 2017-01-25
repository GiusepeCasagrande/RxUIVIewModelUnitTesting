using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Xamvvm;

namespace RxUIVIewModelUnitTesting.ViewModels
{
    public class ViewModelBase : ReactiveObject, INotifyPropertyChanged, ISupportsActivation, IBasePageModel
    {

        bool m_isRunning;
        public bool IsRunning
        {
            get { return m_isRunning; }
            set { this.RaiseAndSetIfChanged(ref m_isRunning, value); }
        }


        public Interaction<string, Unit> ShowError => m_showError;
        public Interaction<string, Unit> ShowSuccess => m_showSuccess;
        public Interaction<string, bool> Confirm => m_confirm;

        public string UrlPathSegment
        {
            get;
            protected set;
        }

        protected readonly ViewModelActivator _viewModelActivator = new ViewModelActivator();

        public ViewModelActivator Activator
        {
            get { return _viewModelActivator; }
        }

        protected readonly Interaction<string, Unit> m_showError;
        protected readonly Interaction<string, Unit> m_showSuccess;
        protected readonly Interaction<string, bool> m_confirm;
        protected readonly IScheduler m_scheduler;

        public ViewModelBase(IScheduler scheduler)
        {
            m_showError = new Interaction<string, Unit>();
            m_showSuccess = new Interaction<string, Unit>();
            m_confirm = new Interaction<string, bool>();
            m_scheduler = scheduler ?? RxApp.MainThreadScheduler;
        }

        protected void ShowGenericError(CompositeDisposable disposables, string errorMessage = "")
        {
            var message = string.IsNullOrEmpty(errorMessage) ? "Something went wrong!" : errorMessage;

            ShowError
                .Handle(message)
                .SubscribeOn(m_scheduler)
                .Subscribe()
                .DisposeWith(disposables);
        }

        protected void ShowSuccessMessage(CompositeDisposable disposables, string successMessage = "")
        {
            var message = string.IsNullOrEmpty(successMessage) ? "Success!" : successMessage;

            ShowSuccess
                .Handle(message)
                .SubscribeOn(m_scheduler)
                .Subscribe()
                .DisposeWith(disposables);
        }

        protected void ShowConfirmation(CompositeDisposable disposables, string message)
        {
            Confirm
                .Handle(message)
                .SubscribeOn(m_scheduler)
                .Subscribe()
                .DisposeWith(disposables);
        }
    }
}