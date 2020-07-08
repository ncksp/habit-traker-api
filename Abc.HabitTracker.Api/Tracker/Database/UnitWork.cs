using Abc.HabitTracker.Api.Tracker.Badge;
using Abc.HabitTracker.Api.Tracker.Habit;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Database
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
    public class UnitWork : IUnitOfWork
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        private IBadgeRepository _badgeRepository;
        private IHabitRepository _habitRepository;
        public IBadgeRepository BadgeRepository
        {
            get
            {
                if(_badgeRepository == null)
                    _badgeRepository = new BadgeRepository(_connection, _transaction);
                
                return _badgeRepository;
            }
        }

        public IHabitRepository HabitRepository
        {
            get
            {
                if (_habitRepository == null)
                    _habitRepository = new HabitRepository(_connection, _transaction);

                return _habitRepository;
            }
        }
        public UnitWork(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _connection.Close();
                }

                disposed = true;
            }
        }
    }
}
