using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Example
{
    [Serializable]
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int LevelsCompleted { get; set; }
        

        public Player(int playerId, string playerName, int score, int levelsCompleted)
        {
            this.PlayerId = playerId;
            this.Name = playerName;
            this.Score = score;
            this.LevelsCompleted = levelsCompleted;
        }

        //public override string ToString()
        //{
        //    return this.Name;
        //}

        public static void Serialize(List<Player> players)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream str = new FileStream("PlayerData.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                bf.Serialize(str, players);
            }
        }

        public static List<Player> Deserialize(List<Player> players)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (Stream stream = File.OpenRead("PlayerData.dat"))
                {
                    players = (List<Player>)bf.Deserialize(stream);
                    
                }
            }
            catch { }

            return players;
        }

    }
}
