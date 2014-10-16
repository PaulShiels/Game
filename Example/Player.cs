using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int LevelsCompleted { get; set; }

        public Player(string playerName, int score, int levelsCompleted)
        {
            this.Name = playerName;
            this.Score = score;
            this.LevelsCompleted = levelsCompleted;
        }

    }
}
