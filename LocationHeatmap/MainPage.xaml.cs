using LocationHeatmap.Controls;
using LocationHeatmap.Services;
using Microsoft.Maui.Controls.Maps;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LocationHeatmap
{
    public partial class MainPage : ContentPage
    {
        private readonly LocationService _locationService;
        private readonly DatabaseService _databaseService;
        private HeatmapLayer _heatmapLayer;

        public MainPage(LocationService locationService, DatabaseService databaseService)
        {
            InitializeComponent();
            _locationService = locationService;
            _databaseService = databaseService;

            _locationService.LocationChanged += OnLocationChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeMapAsync();
        }

        private async Task InitializeMapAsync()
        {
            if (_heatmapLayer == null)
            {
                _heatmapLayer = new HeatmapLayer(map);
                map.MapElements.Add(_heatmapLayer);
            }
            await LoadDataAndCenterMapAsync();
        }

        private async Task LoadDataAndCenterMapAsync()
        {
            var locations = await _databaseService.GetLocationsAsync();
            _heatmapLayer.Points = locations;
            _heatmapLayer.Update();

            if (locations.Any())
            {
                var lastLocation = locations.Last();
                map.MoveToRegion(new MapSpan(new Location(lastLocation.Latitude, lastLocation.Longitude), 0.01, 0.01));
            }
            else
            {
                try
                {
                    var currentLocation = await Geolocation.GetLastKnownLocationAsync();
                    if(currentLocation != null)
                    {
                         map.MoveToRegion(new MapSpan(currentLocation, 0.01, 0.01));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Could not get last known location: {ex.Message}");
                }
            }
        }

        private void OnLocationChanged(object sender, Location e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await LoadDataAndCenterMapAsync();
            });
        }
        
        private void OnMapPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Map.VisibleRegion) && _heatmapLayer != null)
            {
                _heatmapLayer.Update();
            }
        }

        private async void OnStartTrackingClicked(object sender, EventArgs e)
        {
            await _locationService.StartTrackingAsync();
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void OnStopTrackingClicked(object sender, EventArgs e)
        {
            _locationService.StopTracking();
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private async void OnClearDataClicked(object sender, EventArgs e)
        {
            bool shouldClear = await DisplayAlert("Clear Data", "Are you sure you want to delete all location data?", "Yes", "No");
            if (shouldClear)
            {
                await _databaseService.ClearLocationsAsync();
                _heatmapLayer.Points.Clear();
                _heatmapLayer.Update();
            }
        }
    }
}
