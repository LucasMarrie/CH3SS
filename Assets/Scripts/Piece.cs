namespace Chess {

    public abstract class Piece
    {
        int points;
        //for castling and double move pawn
        public bool moved;
        Coord position;
        public Coord Position { get;}
        public Player player {get;}
        
        PieceType type;

        public PieceType Type {get {return type;}}

        public Piece(Player player, int points, PieceType type){
            this.player = player;
            this.points = points;
            this.type = type;
            this.moved = false;
        }


        public abstract Move[] GenerateMoves(Board board);

        public void moveTo(Coord position){
            this.moved = true;
            this.position = position;
        }

        //set position without moving piece
        public void setPosition(Coord position){
            this.position = position;
        }


    }

}