
# Spotify and SignalR WPF
This project is a WPF application which interacts with Spotify and SignalR. It can play/pause songs, view and select playlists and play tracks within playlists.

The API comes with Swagger. Start the project and access URL:PORT or URL:PORT/index.html in order to see the Swagger documentation.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  * [API Project](#api-project)
  * [WPF Project](#wpf-project)
  * [Get Spotify Access Token](#get-spotify-access-token)
- [Code Architecture](#code-architecture)
- [INotifyPropertyChanged](#inotifypropertychanged)

## Prerequisites
- Webserver (or local IIS Express in Visual Studio)
- Spotify Account


## Getting Started
 ### API Project
 
Open up the API project. In the appsettings file, insert the values as described below:  **DeviceID**  The Device ID of the current device playing the music. This can be found by calling the Spotify API.  **AccessToken**  The Access Token which the API should use in order to contact Spotify. You can find more on how to get this in the section "_Get Spotify Access Token_"

### WPF Project
Change APIUrl and HubUrl in the App.config file with values that fit your usage.

### Get Spotify Access Token
[https://developer.spotify.com/console/get-user-player/?market=ES](https://developer.spotify.com/console/get-user-player/?market=ES)
Press GET TOKEN. For ease, just check all the check boxes. The token is valid for 1 hour.


## Code Architecture
![Code Architecture](https://i.imgur.com/VZYSeqK.png)

## INotifyPropertyChanged
To simply and reduce boilerplate code the use Fody's Nuget package (PropertyChanged.Fody) has been decided to handle the triggering of the PropertyChanged event upon property change.
The package changes the auto-properties into properties with a setter which calls the PropertyChanged. This change is done at compile time automatically. The change only happens in classes which inherits and implements the interface *"INotifyPropertyChanged"*
See more at the Github repository [Fody/PropertyChanged](https://github.com/Fody/PropertyChanged#readme)

