using RxUIVIewModelUnitTesting.Services;
using RxUIVIewModelUnitTesting.ViewModels;
using Splat;
using Xamarin.Forms;
using Xamvvm;

namespace RxUIVIewModelUnitTesting
{
    public partial class App : Application
    {
        XamvvmFormsRxUIFactory m_factory;

        public App()
        {
            InitializeComponent();

            Locator.CurrentMutable.RegisterConstant(new FakeAuthenticationSystem(), typeof(IAuthenticationSystem));

            m_factory = new XamvvmFormsRxUIFactory(this);
            XamvvmCore.SetCurrentFactory(m_factory);

            m_factory.RegisterNavigationPage<LoginNavigationViewModel>(() => this.GetPageFromCache<LoginViewModel>());

            MainPage = XamvvmCore.CurrentFactory.GetPageFromCache<LoginNavigationViewModel>() as Page;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
