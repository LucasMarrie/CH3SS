using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess {

    public class Move
    {
        //Note: to play move properly captures must happen first 
        //then all pieces in old positions must be added to an array and removed from board 
        //then all newPositions are replaces with elements in the new array
        public Coord[] oldPositions;
        public Coord[] newPositions;
        public Coord[] captures;

        public List<MoveModifier> modifiers;

        //standard move/capture
        public Move(Coord oldPosition, Coord newPosition) :
            this(new Coord[]{oldPosition}, new Coord[]{newPosition}){}

        //captures that do not put you on the same position i.e. en-passant
        public Move(Coord oldPosition, Coord newPosition, Coord capture) : 
            this(new Coord[]{oldPosition}, new Coord[]{newPosition}, new Coord[]{capture}){}

        //moves that move multiple pieces i.e. castling
        public Move(Coord[] oldPositions, Coord[] newPositions) : 
            this(oldPositions, newPositions, new Coord[0]){}

        //moves that involve multiple captures (possible future addition)
        public Move(Coord[] oldPosition, Coord[] newPosition, Coord[] captures){
            this.oldPositions = oldPosition;
            this.newPositions = newPosition;
            this.captures = captures;
            this.modifiers = new List<MoveModifier>();
        }

        //adds modifier 
        public void AddModifier(MoveModifier modifier){
            modifiers.Add(modifier);
        }


    }
}

