using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess {
    
    public class Board
    {
        Piece[,] grid;
        public bool[,] playableArea;
        public Piece[,] Grid {
            get {return grid;}
        }
        public bool[,] PlayableArea{
            get {return playableArea;}
        }
        int promotionDistance;
        public int PromotionDistance{
            get {return promotionDistance;}
        }
        int width, height;
        int turn;
        Player[] players;
        Move prevMove;
        public Move PrevMove{
            get {return prevMove;}
        }


        public Board(bool[,] boardLayout, Player[] players, int promotionDistance = 0){
            this.width = boardLayout.GetLength(0);
            this.height =  boardLayout.GetLength(1);
            playableArea = boardLayout;
            grid = new Piece[width, height];
            turn = 0;
            this.players = players;
            this.promotionDistance = 0;
        }


        public List<Move> GenerateLegalMoves(){
            List<Move> legalMoves = new List<Move>();
            foreach(Piece piece in Grid){
                if(piece != null && piece.player == CurrentPlayer()){
                    Move[] moves = piece.GenerateMoves(this);
                    foreach(Move move in moves){
                        if(CheckLegalMove(move)) legalMoves.Add(move);
                    }
                }
            }
            return legalMoves;
        }

        private bool CheckLegalMove(Move move){
            Board tempBoard = CloneBoard();
            tempBoard.MakeMove(move);
            return !tempBoard.InCheck(CurrentPlayer(turn));
        }

        private Board CloneBoard(){
            Board tempBoard = new Board(playableArea, players);
            tempBoard.grid = (Piece[,])grid.Clone();
            tempBoard.turn = turn;
            return tempBoard;
        }

        public void MakeMove(Move move){
            foreach(Coord capture in move.captures){
                grid[capture.x, capture.y] = null;
            }
            
            List<Piece> piecesToMove = new List<Piece>();
            foreach(Coord oldPosition in move.oldPositions){
                piecesToMove.Add(grid[oldPosition.x, oldPosition.y]);
                grid[oldPosition.x, oldPosition.y] = null;
            }

            for(int i = 0; i < piecesToMove.Count; i++){
                grid[move.newPositions[i].x, move.newPositions[i].y] = piecesToMove[i];
                piecesToMove[i].moveTo(move.newPositions[i]);
            }
            prevMove = move;
            ProcessModifiers();
            turn++;
        }

        public void ProcessModifiers(){
            foreach(MoveModifier modifier in prevMove.modifiers){
                if(modifier.type == ModificationType.promotion){
                    Debug.Log("Promotion");
                }
            }
        }

        public bool InCheck(Player player){
            foreach((int x, int y, Piece piece) in LoopPieces()){
                if(!player.IsAlly(piece.player)){
                    foreach(Move move in piece.GenerateMoves(this)){
                        foreach(Coord capture in move.captures){
                            if(Grid[capture.x, capture.y].Type == PieceType.king) return true;
                        }
                    }
                }
            }
            return false; 
        }

        public bool IsSpaceAttacked(Coord coord, Player player){
            foreach((int x, int y, Piece piece) in LoopPieces()){
                if(!player.IsAlly(piece.player)){
                    foreach(Move move in piece.GenerateMoves(this)){
                        foreach(Coord newPosition in move.newPositions){
                            if(newPosition == coord) return true;
                        }
                    }
                }
            }
            return false; 
        }

        public bool InBound(Coord coord){
            return InBound(coord.x, coord.y);
        }

        public bool InBound(int x, int y){
            bool inArray = x >= 0 && y >= 0 && x < width && y < height;
            return inArray && playableArea[x, y];; 
        }

        public Player CurrentPlayer(){
            return CurrentPlayer(this.turn);
        }

        public Player CurrentPlayer(int turn){
            return players[turn % players.Length];
        }

        public IEnumerable<(int, int, Piece)> LoopPieces(){
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    yield return (x, y, grid[x, y]);
                }
            }
        }
    }
}