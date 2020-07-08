using Abc.HabitTracker.Api.Tracker.Badge;
using Abc.HabitTracker.Api.Tracker.Badge.BadgeHandler;
using Abc.HabitTracker.Api.Tracker.Constants;
using Abc.HabitTracker.Api.Tracker.Database;
using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Service
{
    public class UserLogged
    {

        private static readonly NpgsqlConnection connection = Connection.GetConnection();
        private static readonly IBadgeRepository repository = new BadgeRepository(connection, null);
        public static void Notify(HabitResponse response)
        {
            Dominating dominating = new Dominating(repository);
            EpicComeback epicComeback = new EpicComeback(repository);
            Workaholic workaholic = new Workaholic(repository);

            LoggerPublisher publisher = new LoggerPublisher();

            publisher.Attach(dominating);
            publisher.Attach(epicComeback);
            publisher.Attach(workaholic);

            publisher.NotifyAll(response);

            publisher.Detach(dominating);
            publisher.Detach(epicComeback);
            publisher.Detach(workaholic);
        }
    }
}
