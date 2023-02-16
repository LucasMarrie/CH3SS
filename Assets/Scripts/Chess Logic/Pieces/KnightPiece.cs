using Chess.StandardMoveset;

namespace Chess.Pieces
{
    public class KnightPiece : Piece
    {
        public KnightPiece(Player player, int points, PieceType type) : base(player, points, type)
        {
        }

        public override Move[] GenerateMoves(Board board)
        {
            return this.KnightMove(board);
        }
    }
}
