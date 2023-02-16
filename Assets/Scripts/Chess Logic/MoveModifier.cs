namespace Chess {

    public class MoveModifier {
        public ModificationType type;
        public Coord[] affectedSpaces;

        public MoveModifier(ModificationType type) : this(type, new Coord[0]){}

        public MoveModifier(ModificationType type, Coord affectedSpace) : this(type, new Coord[]{affectedSpace}){}

        public MoveModifier(ModificationType type, Coord[] affectedSpaces){
            this.type = type;
            this.affectedSpaces = affectedSpaces;
        }

    }
        
    public enum ModificationType{
        charge, //for en-passant
        promotion
    }
}

