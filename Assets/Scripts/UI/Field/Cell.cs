public class Cell
{
    public bool IsFree { get; set; }
    public bool IsContainShip { get; set; }
    public bool IsDamaged { get; set; }

    public Cell()
    {
        IsFree = true;
        IsContainShip = false;
        IsDamaged = false;
    }
}
