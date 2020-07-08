using System;
using System.Collections.Generic;

namespace Abc.HabitTracker.Api.Tracker.Badge
{
    public interface IBadgeRepository
    {
        List<Badge> Find(Guid userID);
        Badge GetBadge(Guid userID, String name);
        void Create(Badge badge);
        void AddWorkaholicSnapshot(Guid userID, DateTime lastLog);
        Int64 CountWorkaholicSnapshot(Guid userID);
    }
}