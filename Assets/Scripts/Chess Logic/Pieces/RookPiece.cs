using Chess.StandardMoveset;

namespace Chess.Pieces
{
    public class RookPiece : Piece
    {
        public RookPiece(Player player, int points, PieceType type) : base(player, points, type)
        {
        }

        public override Move[] GenerateMoves(Board board)
        {
            return this.StraightMove(board);
        }
    }
}
