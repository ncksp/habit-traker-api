using System;
using System.Collections.Generic;

namespace Abc.HabitTracker.Api.Tracker.Habit
{
    public class DayOff
    {
        private static DayOff _dayOff;
        private List<String> DefaultDays = new List<string>();
        public String[] Value { get; private set; }
        private DayOff()
        {
            String[] day = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };


            for (int i = 0; i < day.Length; i++)
            {
                DefaultDays.Add(day[i]);
            }
        }
        public static DayOff GetInstance()
        {
            if (_dayOff == null)
            {
                _dayOff = new DayOff();
            }
            return _dayOff;
        }

        public DayOff SetDayOff(String[] days)
        {
            if (days.Length > 7)
                throw new Exception("Days off must less than 7");

            List<string> temp = new List<string>();
            DefaultDays.ForEach((item) =>
            {
                temp.Add(item);
            });
            foreach (var day in days)
            {
                if(!temp.Contains(day))
                    throw new ArgumentException("Days off invalid days name in : ["+day+"]");
                try
                {
                    temp.Remove(day);
                }
                catch (System.Exception)
                {
                    throw new ArgumentException("Days off cannot be duplicate ");
                }
            }
            this.Value = days;
            return this;
        }
    }
}