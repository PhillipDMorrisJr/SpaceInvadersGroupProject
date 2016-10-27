using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SpaceInvaders.Utils
{
    class SoundFX
    {
        public SoundFX()
        {
            await playBackgroundMusic();
        }

        private static async Task playBackgroundMusic()
        {
            StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Folder = await Folder.GetFolderAsync("MyFolder");
            StorageFile sf = await Folder.GetFileAsync("MyFile.mp3");
            PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
            PlayMusic.Play();
        }
    }
}