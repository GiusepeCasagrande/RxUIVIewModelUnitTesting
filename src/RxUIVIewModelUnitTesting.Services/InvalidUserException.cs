using System;
namespace RxUIVIewModelUnitTesting.Services
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException() : base("Invalid User Or Password")
        {
        }
    }
}
