using Models;
using Models.Spotify.CurrentlyPlaying;
using Services.SignalRService;
using Services.SpotifiyService;
using SpotifyWPF.Utility;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpotifyWPF.ViewModels
{
    public class SpotifyPlayerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Fields/Properties
        private readonly ISpotifyApi _spotifyApi;
        private readonly ISignalRService _signalRService;


        public Progress Progress { get; set; }
        public bool IsPlaying { get; set; }
        public int Volume { get; set; }
        public CurrentlyPlaying CurrentlyPlayingSong { get; set; }

        #endregion

        #region Commands
        public ICommand PlayCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand VolumeCommand { get; set; }
        #endregion


        public SpotifyPlayerViewModel(ISpotifyApi spotifyApi, ISignalRService signalRService)
        {
            _spotifyApi = spotifyApi;
            _signalRService = signalRService;
            Task.Run(() => LoadData());
            LoadCommands();
        }


        /// <summary>
        /// Loads information about the current playing song.
        /// Sets volume and progress according to the information in Spotify.
        /// Starts the progress counter which is responsible for updating the UI whenever
        /// the progress changes.
        /// Starts SignalR connection and subscribes to events.
        /// </summary>
        private async Task LoadData()
        {
            CurrentlyPlayingSong = await _spotifyApi.GetCurrentlyPlayingSongAsync();
            IsPlaying = CurrentlyPlayingSong.is_playing;
            Volume = CurrentlyPlayingSong.device.volume_percent;

            Progress = new Progress()
            {
                Progress_Ms = CurrentlyPlayingSong.progress_ms,
                Duration_Ms = CurrentlyPlayingSong.item.duration_ms
            };

            // Starts progress counter
            _ = Task.Run(() => ProgressCounter());

            await SubscribeToHub();
        }

        /// <summary>
        /// Starts SignalR connection and subscribes to the events:
        /// - SongChanged
        /// - VolumeChanged
        /// - Paused
        /// - Resumed
        /// </summary>
        private async Task SubscribeToHub()
        {
            await _signalRService.ConnectAsync();
            _signalRService.SongChanged += SongChanged;
            _signalRService.VolumeChanged += VolumeChanged;
            _signalRService.Paused += Paused;
            _signalRService.Resumed += Resumed;
        }

        /// <summary>
        /// Initializes commands
        /// </summary>
        private void LoadCommands()
        {
            PlayCommand = new CustomCommand(PlaySong, CanPlaySong);
            PauseCommand = new CustomCommand(PauseSong, CanPauseSong);
            NextCommand = new CustomCommand(NextSong, CanNextSong);
            PreviousCommand = new CustomCommand(PreviousSong, CanPreviousSong);
            VolumeCommand = new CustomCommand(ChangeVolume, CanChangeVolume);
        }

        /// <summary>
        /// Keeps track of the progress by incrementing the played time by 1 every second.
        /// Whenever the track changes it gets information about the new track.
        /// </summary>
        private async void ProgressCounter()
        {
            while (true)
            {
                if (!IsPlaying)
                    continue; // Do not do anything if the song is not playing

                Thread.Sleep(1000);
                Progress oldProgress = Progress;
                Progress = new Progress()
                {
                    Progress_Ms = oldProgress.Progress_Ms + 1000,
                    Duration_Ms = oldProgress.Duration_Ms
                };

                // Updates song info whenever it changes
                if (Progress.Progress_Ms > Progress.Duration_Ms)
                {
                    CurrentlyPlayingSong = await _spotifyApi.GetCurrentlyPlayingSongAsync();
                    await _signalRService.SendSongChangedAsync(CurrentlyPlayingSong);
                }
            }
        }


        #region SignalRMethods

        /// <summary>
        /// Updates the song and its progress
        /// </summary>
        /// <param name="song">The new song to update</param>
        private void SongChanged(CurrentlyPlaying song)
        {
            CurrentlyPlayingSong = song;
            IsPlaying = CurrentlyPlayingSong.is_playing;
            Progress.Progress_Ms = CurrentlyPlayingSong.progress_ms;
            Progress.Duration_Ms = CurrentlyPlayingSong.item.duration_ms;
        }

        /// <summary>
        /// Updates the volume
        /// </summary>
        /// <param name="volume">Volume value to update</param>
        private void VolumeChanged(int volume)
        {
            if (Volume != volume)
                Volume = volume;
        }

        /// <summary>
        /// Sets the player state to paused
        /// </summary>
        private void Paused()
        {
            IsPlaying = false;
        }

        /// <summary>
        /// Sets the player state to resumed
        /// </summary>
        private void Resumed()
        {
            IsPlaying = true;
        }

        #endregion


        #region Command Methods

        #region PlaySong
        /// <summary>
        /// Plays/Resumes the paused song
        /// </summary>
        private async void PlaySong(object obj)
        {
            IsPlaying = true;
            await _spotifyApi.ResumeSongAsync();
            await _signalRService.SendResumedAsync();
        }

        /// <summary>
        /// Checks if it can play song
        /// </summary>
        /// <returns>True if the song is paused</returns>
        private bool CanPlaySong(object obj)
        {
            return !IsPlaying;
        }
        #endregion

        #region PauseSong
        /// <summary>
        /// Pauses the playing song
        /// </summary>
        private async void PauseSong(object obj)
        {
            IsPlaying = false;
            await _spotifyApi.PauseSongAsync();
            await _signalRService.SendPausedAsync();
        }

        /// <summary>
        /// Checks if it can pause the song
        /// </summary>
        /// <returns>True if the song is playing</returns>
        private bool CanPauseSong(object obj)
        {
            return IsPlaying;
        }
        #endregion

        #region NextSong
        /// <summary>
        /// Skips to the next song
        /// </summary>
        private async void NextSong(object obj)
        {
            IsPlaying = true;
            CurrentlyPlaying newSong = await _spotifyApi.NextSongAsync();
            await _signalRService.SendSongChangedAsync(newSong);
        }

        /// <summary>
        /// Check if it can skip to the next song
        /// </summary>
        /// <returns>True if a song is selected</returns>
        private bool CanNextSong(object obj)
        {
            return !string.IsNullOrWhiteSpace(CurrentlyPlayingSong?.item?.id);
        }

        #endregion

        #region PreviousSong
        /// <summary>
        /// Goes to the previous song
        /// </summary>
        private async void PreviousSong(object obj)
        {
            IsPlaying = true;
            CurrentlyPlaying newSong = await _spotifyApi.PreviousSongAsync();
            await _signalRService.SendSongChangedAsync(newSong);
        }

        /// <summary>
        /// Checks if it can play previous song
        /// </summary>
        /// <returns>True if a song is selected</returns>
        private bool CanPreviousSong(object obj)
        {
            return !string.IsNullOrWhiteSpace(CurrentlyPlayingSong?.item?.id);
        }

        #endregion

        #region ChangeVolume
        /// <summary>
        /// Changes the volume in Spotify
        /// </summary>
        private async void ChangeVolume(object obj)
        {
            await _spotifyApi.SetVolumeAsync(Volume);
            await _signalRService.SendVolumeChangedAsync(Volume);
        }

        /// <summary>
        /// Checks if it can change the volume
        /// </summary>
        /// <returns>True if the volume is between 0 and 100</returns>
        private bool CanChangeVolume(object obj)
        {
            return (Volume >= 0 && Volume <= 100);
        }

        #endregion

        #endregion

    }
}
