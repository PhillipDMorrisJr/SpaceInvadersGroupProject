using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;

namespace SpaceInvaders.Model
{
    class HighScoreBoard 
    {
        private List<RankedPlayer> highScores;
        private int AmountOfHighScores = 10;


        public HighScoreBoard()
        {
            this.highScores = new List<RankedPlayer>();
            //read files in parameters of highscores
            this.readFromFile();
            this.writeToFile();
        }

        public void AddPlayerTohighScoreBoard(String name, int score, RankedPlayer.Level level)
        {
            
            RankedPlayer aPlayer = new RankedPlayer(name, score, level);

           this.highScores.Add(aPlayer);

            this.sortHighScores();

            this.trimHighScoreCollection();
        }

        private void trimHighScoreCollection()
        {
            
            while (this.highScores.Count > this.AmountOfHighScores)
            {
                int lastIndex = this.highScores.Count - 1;
                this.highScores.RemoveAt(lastIndex);
            }
        }

        private void sortHighScores()
        {
            var copyOfHighScores = this.highScores.OrderBy(player => player.Score);

            this.highScores.Clear();

            foreach (var variable in copyOfHighScores)
            {
                this.highScores.Add(variable);
            }
        }


        private async void readFromFile()
        {
            var storageFolder =
                ApplicationData.Current.LocalFolder;
            var file = await storageFolder.GetFileAsync("highscores.txt");

            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                var reader = new StreamReader(stream.AsStream());
                var lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
        }

        private async void writeToFile()
        {
            String playerIDs = "";

            foreach (var player in this.highScores)
            {
                playerIDs += player.PlayerID;
            }

            var storageFolder =
                ApplicationData.Current.LocalFolder;
            var sampleFile =
                await storageFolder.CreateFileAsync("highscores.txt",
                    CreationCollisionOption.ReplaceExisting);

            string[] output = {
                
                playerIDs
            };

            var stream = await sampleFile.OpenAsync(FileAccessMode.ReadWrite);

            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    foreach (var line in output)
                    {
                        dataWriter.WriteString(line);
                    }

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose();
        }

    }
}
