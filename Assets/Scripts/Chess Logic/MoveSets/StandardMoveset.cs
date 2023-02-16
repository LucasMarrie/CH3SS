using System.Collections.Generic;
using System.Linq;

namespace Chess.StandardMoveset{

    public static class StandardMoveset
    {

        static readonly Coord[] knightDisplacement = new Coord[]{
            new Coord(1, 2),
            new Coord(2, 1),
            new Coord(2, -1),
            new Coord(1, -2),
            new Coord(-1, -2),
            new Coord(-2, -1),
            new Coord(-2, 1),
            new Coord(-1, 2)
        };

        static readonly Coord[] straightDisplacement = new Coord[]{
            new Coord(1, 0),
            new Coord(0, -1),
            new Coord(-1, 0),
            new Coord(0, 1)
        };

        static readonly Coord[] diagonalDisplacement = new Coord[]{
            new Coord(1, 1),
            new Coord(-1, -1),
            new Coord(-1, 1),
            new Coord(1, -1)
        };

        static readonly PieceType[] castleablePieces = new PieceType[]{
            PieceType.Rook
        };



        
        public static Move[] KnightMove(this Piece piece, Board board){
            List<Move> moves = new List<Move>();
            foreach(Coord displacement in knightDisplacement){
                Coord newPosition = piece.Position + displacement;
                if(board.InBound(newPosition)){
                    Piece pieceOnSpace = board.Grid[newPosition.x, newPosition.y];
                    if(pieceOnSpace == null ){
                        moves.Add(new Move(piece.Position, newPosition));
                    }else if(!pieceOnSpace.player.IsAlly(piece.player)){
                        moves.Add(new Move(piece.Position, newPosition, newPosition));
                    }
                }
            }
            return moves.ToArray();
        }
        //rook type movement but with range, and range = -1 means infinite range
        public static Move[] StraightMove(this Piece piece, Board board, int range = -1){
            List<Move> moves = new List<Move>();
            foreach(Coord displacement in straightDisplacement){
                Coord newPosition = piece.Position + displacement;
                while(board.InBound(newPosition)){
                    Piece pieceOnSpace = board.Grid[newPosition.x, newPosition.y];
                    if(pieceOnSpace != null){
                        if(pieceOnSpace.player.IsAlly(piece.player)){
                            moves.Add(new Move(piece.Position, newPosition, newPosition));
                        }
                        break;
                    }
                    moves.Add(new Move(piece.Position, newPosition));
                    newPosition += displacement;
                    if(range != -1){
                        range--;
                        if(range == 0){
                            break;
                        }
                    }
                }
            }
            return moves.ToArray();
        }


        //bishop type movement but with range, and range = -1 means infinite range
        public static Move[] DiagonalMove(this Piece piece, Board board, int range = -1){
            List<Move> moves = new List<Move>();
            foreach(Coord displacement in diagonalDisplacement){
                Coord newPosition = piece.Position + displacement;
                while(board.InBound(newPosition)){
                    Piece pieceOnSpace = board.Grid[newPosition.x, newPosition.y];
                    if(pieceOnSpace != null){
                        if(pieceOnSpace.player.IsAlly(piece.player)){
                            moves.Add(new Move(piece.Position, newPosition, newPosition));
                        }
                        break;
                    }
                    moves.Add(new Move(piece.Position, newPosition));
                    newPosition += displacement;
                    if(range != -1){
                        range--;
                        if(range == 0){
                            break;
                        }
                    }
                }
            }
            return moves.ToArray();
        }

        public static Move[] PawnWalk(this Piece piece, Board board, int chargeRange = 2){
            List<Move> moves = new List<Move>();
            Coord displacement = piece.player.forwardDirection;
            Coord newPosition = piece.Position + displacement;
            int moveAllowance = piece.moved ? 1 : chargeRange;
            List<Coord> inBetweenMoves = new List<Coord>();
            for(int i = 0; i < moveAllowance; i++){
                if(board.InBound(newPosition)){
                    Piece pieceOnSpace = board.Grid[newPosition.x, newPosition.y];
                    if(pieceOnSpace == null){
                        Move move = new Move(piece.Position, newPosition);
                        if(i > 0){
                            MoveModifier modifier = new MoveModifier(ModificationType.charge, inBetweenMoves.ToArray());
                            move.AddModifier(modifier);
                        }
                        moves.Add(move);
                        inBetweenMoves.Add(newPosition);
                    }
                    else{
                        break;
                    }
                }
                newPosition += displacement;
            }
            return moves.ToArray();
        }

        public static Move[] PawnAttack(this Piece piece, Board board){
            List<Move> moves = new List<Move>();
            Coord forward = piece.player.forwardDirection;
            Coord[] attackDisplacements = new Coord[]{
                forward + forward.invert(),
                forward - forward.invert()
            };
            foreach(Coord displacement in attackDisplacements){
                Coord newPosition = piece.Position + displacement;
                if(board.InBound(newPosition)){
                    Piece pieceOnSpace = board.Grid[newPosition.x, newPosition.y];
                    if(pieceOnSpace != null && !pieceOnSpace.player.IsAlly(piece.player)){
                        moves.Add(new Move(piece.Position, newPosition, newPosition));
                    }else if(pieceOnSpace == null){
                        //check for en passant captures
                        foreach(MoveModifier modifier in board.PrevMove.modifiers){
                            if(modifier.type == ModificationType.charge && modifier.affectedSpaces.Contains(newPosition)){
                                moves.Add(new Move(piece.Position, newPosition, board.PrevMove.newPositions[0]));
                            }
                        }
                    }
                }
            }
            return moves.ToArray();
        }

        public static void PawnPromotion(this Piece piece, Board board, Move[] moves){
            foreach(Move move in moves){
                Coord forward = piece.player.forwardDirection;
                bool canPromote;
                if(forward.x != 0){
                    int promotionPosition = forward.x > 0 ? board.PromotionDistance : board.Grid.GetLength(0) - board.PromotionDistance - 1;
                    canPromote = move.newPositions[0].x == promotionPosition;
                }else{
                    int promotionPosition = forward.y > 0 ? board.PromotionDistance : board.Grid.GetLength(1) - board.PromotionDistance - 1;
                    canPromote = move.newPositions[0].y == promotionPosition;
                } 
                
                if(canPromote){
                    move.AddModifier(new MoveModifier(ModificationType.promotion, move.newPositions[0]));
                }
            }
        } 

        public static Move[] Castling(this Piece piece, Board board){
            //check if piece has moved or is in check
            if (piece.moved || board.IsSpaceAttacked(piece.Position, piece.player)) return new Move[0];

            List<Move> moves = new List<Move>();
            Coord forward = piece.player.forwardDirection;
            Coord leftCastle = -forward.invert();
            Coord rightCastle = forward.invert();

            Move leftCastleMove = piece.OneSideCastle(board, leftCastle);
            if(leftCastleMove != null){
                moves.Add(leftCastleMove);
            }
            Move RightCastleMove = piece.OneSideCastle(board, rightCastle);
            if(RightCastleMove != null){
                moves.Add(RightCastleMove);
            }
            //destination of castle is verified by legal move verification
            return moves.ToArray();
        }

        private static Move OneSideCastle(this Piece piece, Board board, Coord direction){
            Coord newPosition = piece.Position + direction;
            int distance = 1;

            //castle is blocked by attack
            if(board.IsSpaceAttacked(newPosition, piece.player)) return null;
            //targetLocation is out of range
            if(!board.InBound(newPosition + direction)) return null;

            //check for castleable pieces
            while(board.InBound(newPosition)){
                Piece pieceOnSpace = board.Grid[newPosition.x, newPosition.y];
                if(pieceOnSpace != null){
                    if(pieceOnSpace.player.IsAlly(piece.player) && castleablePieces.Contains(pieceOnSpace.Type) && !pieceOnSpace.moved){
                        Coord[] oldPositions = {pieceOnSpace.Position, newPosition};
                        Coord[] newPositions = {pieceOnSpace.Position + 2 * direction, newPosition + direction};
                        return new Move(oldPositions, newPositions);
                    }
                    break;
                }
                newPosition += direction;
                distance++;
            }
            return null;
        }

    }
   
}
