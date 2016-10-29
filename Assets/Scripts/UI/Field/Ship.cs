public abstract class Ship
{
    private Field _field;

    public int X { get; private set; }
    public int Y { get; private set; }

    public Ship(Field field)
    {
        _field = field;
    }

    public bool SetPosition(int x, int y)
    {
        X = x;
        Y = y;

        return true;
    }
}
