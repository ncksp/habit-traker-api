using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker.Response;
using Abc.HabitTracker.Api.Tracker.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Abc.HabitTracker.Api.Tracker.Badge.BadgeHandler
{
    public class Workaholic : Observer<HabitResponse>
    {

        private readonly string Name = "Workaholic";
        private readonly string Desc = "Doing some works on daysoff";
        private IBadgeRepository _badgeRepository;
        public Workaholic(IBadgeRepository badgeRepository)
        {
            this._badgeRepository = badgeRepository;
        }
        public void Update(HabitResponse e)
        {
            IEnumerable<DateTime> logs = e.Logs;
            DateTime lastLog = logs.Last();
            String daySplit = lastLog.DayOfWeek.ToString().Substring(0, 3);

            int idx = Array.IndexOf(e.DaysOff, daySplit);
            if (idx > -1)
                return;

            _badgeRepository.AddWorkaholicSnapshot(e.UserID, lastLog);

            Int64 data = _badgeRepository.CountWorkaholicSnapshot(e.UserID);
            if (data < 10)
                return;

            var current = _badgeRepository.GetBadge(e.UserID, Name);
            if (current != null)
                return;

            Badge badge = new Badge(
               Guid.NewGuid(),
               Name,
               Desc,
               e.UserID,
               DateTime.Now);

            _badgeRepository.Create(badge);
        }
    }
}
