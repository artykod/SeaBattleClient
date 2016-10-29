using System.Collections.Generic;

public class Field
{
    private LinkedList<Ship> _allShips = new LinkedList<Ship>();

    public Cell[,] Cells { get; private set; }
    public int Size { get; private set; }
    public ShipOne[] Ships1 { get; private set; }
    public ShipTwo[] Ships2 { get; private set; }
    public ShipThree[] Ships3 { get; private set; }
    public ShipFour[] Ships4 { get; private set; }

    public Field(int size = 10)
    {
        Cells = new Cell[size, size];

        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
            {
                Cells[i, j] = new Cell();
            }

        AddShips(Ships1 = new ShipOne[4]);
        AddShips(Ships2 = new ShipTwo[3]);
        AddShips(Ships3 = new ShipThree[2]);
        AddShips(Ships4 = new ShipFour[1]);
    }

    private void AddShips(Ship[] ships)
    {
        for (int i = 0; i < ships.Length; i++)
            _allShips.AddLast(ships[i]);
    }
}