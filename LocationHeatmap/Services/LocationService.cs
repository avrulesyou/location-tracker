using LocationHeatmap.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LocationHeatmap.Services
{
    public class LocationService
    {
        private readonly DatabaseService _databaseService;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isTracking;

        public event EventHandler<Location> LocationChanged;

        public LocationService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task StartTrackingAsync()
        {
            if (_isTracking) return;
            var status = await CheckAndRequestLocationPermission();
            if (status != PermissionStatus.Granted)
            {
                Console.WriteLine("Location permission not granted.");
                return;
            }

            _isTracking = true;
            _cancellationTokenSource = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                        var location = await Geolocation.GetLocationAsync(request, _cancellationTokenSource.Token);

                        if (location != null)
                        {
                            var locationPoint = new LocationPoint
                            {
                                Latitude = location.Latitude,
                                Longitude = location.Longitude,
                                Timestamp = DateTime.UtcNow
                            };
                            await _databaseService.SaveLocationAsync(locationPoint);
                            LocationChanged?.Invoke(this, location);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting location: {ex.Message}");
                    }
                    await Task.Delay(TimeSpan.FromSeconds(30), _cancellationTokenSource.Token);
                }
            }, _cancellationTokenSource.Token);
        }

        public void StopTracking()
        {
            if (!_isTracking) return;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _isTracking = false;
        }

        private async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted) return status;
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;
        }
    }
}
