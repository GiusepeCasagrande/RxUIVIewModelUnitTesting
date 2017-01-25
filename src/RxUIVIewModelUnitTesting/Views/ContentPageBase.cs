using System;
using System.Reactive;
using System.Reactive.Disposables;
using Acr.UserDialogs;
using ReactiveUI;
using ReactiveUI.XamForms;
using RxUIVIewModelUnitTesting.ViewModels;
using Xamvvm;

namespace RxUIVIewModelUnitTesting.Views
{
   public class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel>, IBasePage<TViewModel> where TViewModel : ViewModelBase
    {
        public ContentPageBase()
        {
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this
                    .WhenAnyValue(x => x.ViewModel.IsRunning)
                    .Subscribe(isRunnig =>
                {
                    if (isRunnig)
                        UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                    else
                        UserDialogs.Instance.HideLoading();
                });

                this
                    .ViewModel?
                    .ShowError
                    .RegisterHandler(
                        interaction =>
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.ShowError(interaction.Input, 3000);
                            interaction.SetOutput(new Unit());
                        })
                    .DisposeWith(disposables);

                this
                    .ViewModel?
                    .ShowSuccess
                    .RegisterHandler(
                        interaction =>
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.ShowSuccess(interaction.Input, 3000);
                            interaction.SetOutput(new Unit());
                        })
                    .DisposeWith(disposables);

                this
                    .ViewModel?
                    .Confirm
                    .RegisterHandler(
                        async interaction =>
                        {
                            var isOk = await this.DisplayAlert(
                                        "Confirmation",
                                        interaction.Input,
                                        "Ok",
                                        "Cancel");
                            interaction.SetOutput(isOk);
                        });
            });
        }
    }
}

