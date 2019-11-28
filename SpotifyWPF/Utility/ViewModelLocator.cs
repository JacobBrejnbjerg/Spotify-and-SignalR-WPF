using GalaSoft.MvvmLight.Ioc;
using Services.Settings;
using Services.SignalRService;
using Services.SpotifiyService;
using SpotifyAPI.Lib;
using SpotifyWPF.ViewModels;
using System.Configuration;

namespace SpotifyWPF.Utility
{
    public class ViewModelLocator
    {
        private readonly string _apiUrl;
        private readonly string _hubUrl;
        private readonly string _hubName;

        public ViewModelLocator()
        {
            // Get APIUrl from App.config
            _apiUrl = ConfigurationManager.AppSettings["APIUrl"].ToString();
            _hubUrl = ConfigurationManager.AppSettings["HubUrl"].ToString();
            _hubName = ConfigurationManager.AppSettings["HubName"].ToString();

            SimpleIoc.Default.Register<HttpCaller>(() => new HttpCaller(_apiUrl));
            SimpleIoc.Default.Register<ISpotifyApi, SpotifyApi>();
            
            SimpleIoc.Default.Register<HubSettings>(() => new HubSettings(_hubUrl, _hubName));
            SimpleIoc.Default.Register<ISignalRService, SignalRService>();

            SimpleIoc.Default.Register<SpotifyOverviewViewModel>(true);
            SimpleIoc.Default.Register<SpotifyPlayerViewModel>(true);
            SimpleIoc.Default.Register<SpotifyPlaylistsViewModel>(true);
        }

        public SpotifyOverviewViewModel SpotifyOverviewViewModel => SimpleIoc.Default.GetInstance<SpotifyOverviewViewModel>();
        public SpotifyPlayerViewModel SpotifyPlayerViewModel => SimpleIoc.Default.GetInstance<SpotifyPlayerViewModel>();
        public SpotifyPlaylistsViewModel SpotifyPlaylistsViewModel => SimpleIoc.Default.GetInstance<SpotifyPlaylistsViewModel>();
    }
}
