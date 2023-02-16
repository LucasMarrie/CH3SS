using Chess.StandardMoveset;

namespace Chess.Pieces
{
    public class BishopPiece : Piece
    {
        public BishopPiece(Player player, int points, PieceType type) : base(player, points, type)
        {
        }

        public override Move[] GenerateMoves(Board board)
        {
            return this.DiagonalMove(board);
        }
    }
}