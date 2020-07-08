using Abc.HabitTracker.Api.Tracker.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Service
{
    public interface IObservable<T>
    {
        void Attach(Observer<T> o);
        void Detach(Observer<T> o);
        void NotifyAll(T e);
    }

    public interface Observer<T>
    {
        public void Update(T e);
    }
}
