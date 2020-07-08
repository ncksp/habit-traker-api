using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker.Response;
using Abc.HabitTracker.Api.Tracker.Service;

namespace Abc.HabitTracker.Api.Tracker.Badge.BadgeHandler
{
    public class EpicComeback : Observer<HabitResponse>
    {
        private IBadgeRepository _badgeRepository;
        public EpicComeback(IBadgeRepository badgeRepository)
        {
            this._badgeRepository = badgeRepository;
        }
        public void Update(HabitResponse e)
        {
        }
    }
}