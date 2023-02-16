using System.Collections.Generic;
using UnityEngine;

namespace Chess {

    public class Player 
    {
        int playerNumber;
        public int teamNumber;
        public Coord forwardDirection;

        public Player(int playerNumber, int teamNumber, Coord forwardDirection)
        {
            Debug.Assert(Mathf.Abs(forwardDirection.x) + Mathf.Abs(forwardDirection.y) == 1, "forwardDirection must be a unit vector");
            this.playerNumber = playerNumber;
            this.teamNumber = teamNumber;
            this.forwardDirection = forwardDirection;
        }
        
        public bool IsForward(Coord direction)
        {
            return direction == forwardDirection;
        }


        public bool IsAlly(Player other)
        {
            return teamNumber == other.teamNumber;
        }

        public static bool operator ==(Player a, Player b)
        {
            return a.playerNumber == b.playerNumber;
        }

        public static bool operator !=(Player a, Player b)
        {
            return a.playerNumber != b.playerNumber;
        }

        public override bool Equals(object obj)
        {
            return obj is Player player && player.playerNumber == playerNumber;
        }

        public override int GetHashCode()
        {
            return playerNumber + teamNumber * 1000;
        }

    }
}
