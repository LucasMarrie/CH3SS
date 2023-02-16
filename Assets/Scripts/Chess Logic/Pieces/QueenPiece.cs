using System.Collections.Generic;
using Chess.StandardMoveset;

namespace Chess.Pieces
{
    public class QueenPiece : Piece
    {
        public QueenPiece(Player player, int points, PieceType type) : base(player, points, type)
        {
        }

        public override Move[] GenerateMoves(Board board)
        {
            List<Move> moves = new List<Move>();
            moves.AddRange(this.DiagonalMove(board));
            moves.AddRange(this.StraightMove(board));
            return moves.ToArray();
        }
    }
}