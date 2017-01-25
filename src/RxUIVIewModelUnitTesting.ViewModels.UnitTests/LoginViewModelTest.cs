using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RxUIVIewModelUnitTesting.Services;
using Xamvvm;

namespace RxUIVIewModelUnitTesting.ViewModels.UnitTests
{
    public class LoginViewModelTest
    {
        LoginViewModel m_actual;
        string m_error = string.Empty;

        [SetUp]
        public void Setup()
        {
            Splat.ModeDetector.InUnitTestRunner();

            var factory = new XamvvmMockFactory();
            XamvvmCore.SetCurrentFactory(factory);


            var authenticationSystem = new FakeAuthenticationSystem();

            m_actual = new LoginViewModel(authenticationSystem, CurrentThreadScheduler.Instance);
            m_actual
              .ShowError
              .RegisterHandler(
                  interaction =>
                  {
                      m_error = interaction.Input;
                      interaction.SetOutput(new Unit());
                  });

            m_actual.Activator.Activate();
            m_error = string.Empty;
        }

        [Test]
        public async Task Login_InvalidUserOrPassword_InvalidUserMessage()
        {
            m_actual.UserName = "InvalidUserName";
            m_actual.Password = "InvalidPassword";

            Assert.IsFalse(m_actual.IsRunning);

            try
            {
                await m_actual.LoginCommand.Execute(Unit.Default);
                Assert.IsTrue(m_actual.IsRunning);
            }
            catch (Exception)
            {
            }

            Assert.AreEqual("Invalid User Or Password", m_error);
        }

        [Test]
        public async Task Login_ValidUserAndPassword_NavigateToNextPage()
        {
            m_actual.UserName = "ValidUser";
            m_actual.Password = "ValidPassword";
            await m_actual.LoginCommand.Execute(Unit.Default);

            Assert.IsTrue(string.IsNullOrEmpty(m_error));

            Assert.True(XamvvmMockFactory.LastActionSuccess);
            Assert.AreEqual(XamvvmMockFactory.XammvvmAction.PagePushed, XamvvmMockFactory.LastAction);
            await Task.Delay(500);
            Assert.IsInstanceOf<SomeViewModel>(XamvvmMockFactory.TargetPageModel);
        }
    }
}
