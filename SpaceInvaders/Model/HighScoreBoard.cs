using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SpaceInvaders.Model
{
    internal class HighScoreBoard
    {
        private readonly int AmountOfHighScores = 10;
        private readonly List<RankedPlayer> highScores;


        public HighScoreBoard()
        {
            highScores = new List<RankedPlayer>();
            //read files in parameters of highscores
            readFromFile();
            writeToFile();
        }

        public void AddPlayerTohighScoreBoard(string name, int score, RankedPlayer.Level level)
        {
            var aPlayer = new RankedPlayer(name, score, level);

            highScores.Add(aPlayer);

            sortHighScores();

            trimHighScoreCollection();
        }

        private void trimHighScoreCollection()
        {
            while (highScores.Count > AmountOfHighScores)
            {
                var lastIndex = highScores.Count - 1;
                highScores.RemoveAt(lastIndex);
            }
        }

        private void sortHighScores()
        {
            var copyOfHighScores = highScores.OrderBy(player => player.Score);

            highScores.Clear();

            foreach (var variable in copyOfHighScores)
            {
                highScores.Add(variable);
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
            var playerIDs = "";

            foreach (var player in highScores)
            {
                playerIDs += player.PlayerID;
            }

            var storageFolder =
                ApplicationData.Current.LocalFolder;
            var sampleFile =
                await storageFolder.CreateFileAsync("highscores.txt",
                    CreationCollisionOption.ReplaceExisting);

            string[] output =
            {
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