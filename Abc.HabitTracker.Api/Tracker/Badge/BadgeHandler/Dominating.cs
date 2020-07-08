using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker.Response;
using Abc.HabitTracker.Api.Tracker.Service;
using System;

namespace Abc.HabitTracker.Api.Tracker.Badge.BadgeHandler
{
    public class Dominating : Observer<HabitResponse>
    {
        private readonly string Name = "Dominating";
        private readonly string Desc = "4+ streak";
        private IBadgeRepository _badgeRepository;
        public Dominating(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }
        public void Update(HabitResponse e)
        {
            if (e.CurrentStreak < 4)
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