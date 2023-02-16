using System.Collections.Generic;
using Chess.StandardMoveset;

namespace Chess.Pieces
{
    public class KingPiece : Piece
    {
        public KingPiece(Player player, int points, PieceType type) : base(player, points, type)
        {
        }

        public override Move[] GenerateMoves(Board board)
        {
            List<Move> moves = new List<Move>();
            moves.AddRange(this.DiagonalMove(board, 1));
            moves.AddRange(this.StraightMove(board, 1));
            moves.AddRange(this.Castling(board));
            return moves.ToArray();
        }
    }
}