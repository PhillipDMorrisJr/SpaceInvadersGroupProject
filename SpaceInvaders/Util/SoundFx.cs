using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.Util
{
    internal static class SoundFx
    {
        public static async void PlaySound()
        {
            var mysong = new MediaElement();
            var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("phasers3.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mysong.SetSource(stream, file.ContentType);
            mysong.Play();
        }
    }
}