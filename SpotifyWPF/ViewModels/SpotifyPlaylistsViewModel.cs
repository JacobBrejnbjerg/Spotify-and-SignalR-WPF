using Models.Spotify.CurrentlyPlaying;
using Models.Spotify.Playlist;
using Models.Spotify.Track;
using Services.SignalRService;
using Services.SpotifiyService;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpotifyWPF.Utility;
using System.ComponentModel;

namespace SpotifyWPF.ViewModels
{
    public class SpotifyPlaylistsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Fields/Properties
        private readonly ISpotifyApi _spotifyApi;
        private readonly ISignalRService _signalRService;


        public Tracks Tracks { get; set; }
        public Playlists UserPlaylists { get; set; }

        public PlaylistItem SelectedPlaylist { get; set; }
        
        public TrackItem SelectedTrack { get; set; }
        
        #endregion

        #region Commands

        public ICommand ChangePlaylistCommand { get; set; }
        public ICommand PlayTrackCommand { get; set; }

        #endregion

        public SpotifyPlaylistsViewModel(ISpotifyApi spotifyApi, ISignalRService signalRService)
        {
            _spotifyApi = spotifyApi;
            _signalRService = signalRService;

            Task.Run(() => LoadData());

            PlayTrackCommand = new CustomCommand(PlayTrack, CanPlayTrack);
            ChangePlaylistCommand = new CustomCommand(ChangePlaylist, CanChangePlaylist);
        }


        /// <summary>
        /// Connects to SignalR SongHub and subscribes to 
        /// PlaylistChanged and SongChanged events.
        /// At the end it initializes the users playlists
        /// </summary>
        private async Task LoadData()
        {
            await _signalRService.ConnectAsync();
            _signalRService.PlaylistChanged += PlaylistChanged;
            _signalRService.SongChanged += SongChanged;
            UserPlaylists = await _spotifyApi.GetPlaylistsAsync();
        }


        #region SignalR Methods

        /// <summary>
        /// Update selected song whenever a new song comes on
        /// </summary>
        /// <param name="newSong">The song which is the new one</param>
        private void SongChanged(CurrentlyPlaying newSong)
        {
            // If newSong is null do nothing
            if (newSong == null || Tracks?.items == null)
                return;

            // Finds new track in playlist
            TrackItem newTrack = Tracks.items.Find(t => t.track.uri == newSong.item.uri);

            // Sets the new track as the SelectedTrack
            SelectedTrack = newTrack;
        }

        /// <summary>
        /// Updates selected playlist whenever a new playlist is selected
        /// </summary>
        /// <param name="playlist">Playlist which is the newly selected one</param>
        private async void PlaylistChanged(PlaylistItem playlist)
        {
            // Find playlist in UserPlaylists
            PlaylistItem selectPlaylist = UserPlaylists.items.FirstOrDefault(up => up.id == playlist.id);

            // Set playlist to the found one
            SelectedPlaylist = selectPlaylist;

            // Load the new tracks
            Tracks = await _spotifyApi.GetTracksAsync(selectPlaylist.id);
        }

        #endregion


        #region Command Methods

        #region Play Track

        /// <summary>
        /// Plays the newly selected track
        /// </summary>
        private async void PlayTrack(object obj)
        {
            int indexOfTrack = Tracks.items.IndexOf(SelectedTrack);
            CurrentlyPlaying newSong = await _spotifyApi.PlaySongAsync(SelectedPlaylist.uri, indexOfTrack);
            await _signalRService.SendSongChangedAsync(newSong);
        }

        /// <summary>
        /// Checks if it can play the new track
        /// </summary>
        /// <returns>True if a track is selected</returns>
        private bool CanPlayTrack(object obj)
        {
            return SelectedTrack != null;
        }

        #endregion

        #region ChangePlaylist

        /// <summary>
        /// Triggers the playlist changed event whenever a new playlist is selected
        /// </summary>
        private async void ChangePlaylist(object obj)
        {
            await _signalRService.SendPlaylistChangedAsync(SelectedPlaylist);
        }

        /// <summary>
        /// Checks if it is possible to change playlist
        /// </summary>
        /// <returns>True if a playlist is selected</returns>
        private bool CanChangePlaylist(object obj)
        {
            return SelectedPlaylist != null;
        }

        #endregion

        #endregion
    }
}
