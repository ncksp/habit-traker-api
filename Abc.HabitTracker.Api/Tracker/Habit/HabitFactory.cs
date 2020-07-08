using System;

namespace Abc.HabitTracker.Api.Tracker.Habit
{
    public class HabitFactory
    {
        public static Habit Create(RequestData data, Guid userId)
        {
            UserRepository user = new UserRepository();
            var u = user.Find(userId);

            if (u == null)
                return null;

            Habit newData = new Habit
            {
                DaysOff = DayOff.GetInstance().SetDayOff(data.DaysOff),
                Name = new Name(data.Name),
                UserID = userId,
                ID = Guid.NewGuid(),
                CreatedAt = DateTime.Now
            };
            return newData;
        }
    }
}