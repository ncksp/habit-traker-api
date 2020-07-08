using Abc.HabitTracker.Api.Tracker.Response;
using System;
using System.Collections.Generic;

namespace Abc.HabitTracker.Api.Tracker.Habit
{
    public interface IHabitRepository
    {
        List<HabitResponse> GetHabits(Guid userID);
        HabitResponse Find(Guid userId, Guid id);
        void Create(Habit habit);
        void Update(Guid id, RequestData habit, Guid userID);
        void Delete(Guid id, Guid userId);
        void AddLog(Guid userId, Guid id);
    }
}