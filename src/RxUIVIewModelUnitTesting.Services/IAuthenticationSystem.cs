using System;
using System.Threading.Tasks;

namespace RxUIVIewModelUnitTesting.Services
{
    public interface IAuthenticationSystem
    {
        Task<string> LoginAsync(string userName, string password);
    }
}
