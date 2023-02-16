public class Coord {

    public int x, y;

    public Coord(int x, int y){
        this.x = x;
        this.y = y;
    }

    public Coord invert(){
        return new Coord(y, x);
    }

    public static bool operator ==(Coord a, Coord b){
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coord a, Coord b){
        return !(a == b);
    }


    public static Coord operator +(Coord a, Coord b){
        return new Coord(a.x + b.x, a.y + b.y);
    }

    public static Coord operator -(Coord a, Coord b){
        return new Coord(a.x - b.x, a.y - b.y);
    }

    public static Coord operator -(Coord a){
        return new Coord(-a.x , -a.y);
    }

    public static Coord operator *(int b, Coord a){
        return new Coord(a.x * b, a.y * b);
    }

    
    public override bool Equals(object obj){
        if(obj is Coord){
            return this == (Coord)obj;
        }
        return false;
    }

    public override int GetHashCode(){
        return x.GetHashCode() ^ y.GetHashCode();
    }

}
