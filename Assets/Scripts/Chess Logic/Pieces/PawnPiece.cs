using System.Collections.Generic;
using Chess.StandardMoveset;

namespace Chess.Pieces {

    public class pawnPiece : Piece
    {
        public pawnPiece(Player player, int points, PieceType type) : base(player, points, type)
        {
        }

        public override Move[] GenerateMoves(Board board)
        {
            List<Move> moves = new List<Move>();
            moves.AddRange(this.PawnWalk(board));
            moves.AddRange(this.PawnAttack(board));
            Move[] moveArray = moves.ToArray();
            this.PawnPromotion(board, moveArray);
            return moveArray;
        }
    }

}
