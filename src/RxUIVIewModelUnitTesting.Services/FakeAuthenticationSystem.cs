using System;
using System.Threading.Tasks;

namespace RxUIVIewModelUnitTesting.Services
{
    public class FakeAuthenticationSystem : IAuthenticationSystem
    {
        public FakeAuthenticationSystem()
        {
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            await Task.Delay(2000);

            if (userName != "me" && password != "myself")
                throw new InvalidUserException();

            return "Token";
        }
    }
}
