using LocationHeatmap.Models;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LocationHeatmap.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _isInitialized = false;

        private async Task InitializeAsync()
        {
            if (_isInitialized) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "LocationData.db");
            _database = new SQLiteAsyncConnection(databasePath);
            await _database.CreateTableAsync<LocationPoint>();
            _isInitialized = true;
        }

        public async Task<List<LocationPoint>> GetLocationsAsync()
        {
            await InitializeAsync();
            return await _database.Table<LocationPoint>().ToListAsync();
        }

        public async Task<int> SaveLocationAsync(LocationPoint location)
        {
            await InitializeAsync();
            return await _database.InsertAsync(location);
        }

        public async Task<int> ClearLocationsAsync()
        {
            await InitializeAsync();
            return await _database.DeleteAllAsync<LocationPoint>();
        }
    }
}
