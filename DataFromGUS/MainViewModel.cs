using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace DataFromGUS
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;

        public MainViewModel()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            _httpClient = serviceProvider.GetService<HttpClient>();
            Areas = new ObservableCollection<AreaModel>();
            LoadData();
        }

        private ObservableCollection<AreaModel> _areas;
        public ObservableCollection<AreaModel> Areas
        {
            get { return _areas; }
            set
            {
                _areas = value;
                OnPropertyChanged(nameof(Areas));
            }
        }

        private async void LoadData()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api-dbw.stat.gov.pl/api/1.1.0/area/area-area");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var areas = JsonConvert.DeserializeObject<List<AreaModel>>(json);
                    foreach (var area in areas)
                    {
                        area.LevelNameColor = GetLevelNameBrush(area.LevelName);
                        Areas.Add(area);
                    }
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd podczas pobierania danych z API GUS.");
                }
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd podczas pobierania danych z API GUS.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private SolidColorBrush GetLevelNameBrush(string levelName)
        {
            switch (levelName)
            {
                case "Temat":
                    return new SolidColorBrush(Colors.Green);
                case "Zakres informacyjny":
                    return new SolidColorBrush(Colors.Red);
                case "Dziedzina":
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }
    }
}
