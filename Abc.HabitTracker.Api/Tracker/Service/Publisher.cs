using Abc.HabitTracker.Api.Tracker.Badge.BadgeHandler;
using Abc.HabitTracker.Api.Tracker.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Service
{
    public class LoggerPublisher : IObservable<HabitResponse>
    {
        private List<Observer<HabitResponse>> observers = new List<Observer<HabitResponse>>();

        public void Attach(Observer<HabitResponse> o)
        {
            observers.Add(o);
        }

        public void Detach(Observer<HabitResponse> o)
        {
            observers.Remove(o);
        }

        public void NotifyAll(HabitResponse e)
        {
            foreach(Observer<HabitResponse> obj in observers)
            {
                obj.Update(e);
            }
        }
    }
}
