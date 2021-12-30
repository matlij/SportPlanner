using ModelsCore.Enums;
using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Repository.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SportPlanner.Repository
{
    public class AsyncLazy<T>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }

    public class UserDatabase : ILocalStorage<User>
    {
        private static SQLiteAsyncConnection _database;

        public static readonly AsyncLazy<UserDatabase> Instance = new AsyncLazy<UserDatabase>(async () =>
        {
            var instance = new UserDatabase();
            CreateTableResult result = await _database.CreateTableAsync<User>();
            return instance;
        });

        public UserDatabase()
        {
            _database = new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags);
        }

        public Task<List<User>> GetAll()
        {
            return _database.Table<User>().ToListAsync();
        }


        public Task<User> Get(Guid id)
        {
            return _database.Table<User>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<CrudResult> Upsert(User user)
        {
            var storedUser = await Get(user.Id);
            var rowsChanged = storedUser is null
                ? await _database.InsertAsync(user)
                : await _database.UpdateAsync(user);

            return rowsChanged == 0
                ? CrudResult.Error
                : CrudResult.Ok;
        }

        public async Task<CrudResult> Delete(Guid id)
        {
            var user = await Get(id);
            var rowsDeleted = await _database.DeleteAsync(user);
            return rowsDeleted == 0
                ? CrudResult.NotFound
                : CrudResult.Ok;
        }
    }
}
