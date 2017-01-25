using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using ReactiveUI;
using RxUIVIewModelUnitTesting.ViewModels;
using Xamarin.Forms;

namespace RxUIVIewModelUnitTesting.Views
{
    public partial class LoginView : ContentPageBase<LoginViewModel>
    {
        public LoginView()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, x => x.UserName, x => x.UserName.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Password, x => x.Password.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.LoginCommand, x => x.Login.Command).DisposeWith(disposables);
            });
        }
    }
}