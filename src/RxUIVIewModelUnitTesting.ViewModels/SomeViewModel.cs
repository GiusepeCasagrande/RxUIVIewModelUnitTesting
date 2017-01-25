using System;
using System.Reactive.Concurrency;

namespace RxUIVIewModelUnitTesting.ViewModels
{
    public class SomeViewModel : ViewModelBase
    {
        public SomeViewModel() : this(null)
        {
        }

        public SomeViewModel(IScheduler scheduler = null) : base(scheduler)
        {
        }
    }
}
