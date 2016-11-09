using UnityEngine;
using System.Collections.Generic;

public class ShipCountViewContext : NeastedBindContext
{
    public Bind<int> Mode { get; set; }
    public Bind<int> Count { get; set; }

    public ShipCountViewContext()
    {
        Count.OnValueChanged += (val) => Mode.Value = Count.Value > 0 ? 0 : 2;
    }

    public void Init(int count, int mode)
    {
        Count.Value = count;
        Mode.Value = mode;
    }
}

public class ShipViewContext : NeastedBindContext
{
    public Bind<bool> Visible;
    public Bind<float> Rotation;
    public Bind<Vector3> Position;

    public void FetchFromModel(List<ShipModel> models, int index)
    {
        Visible.Value = models.Count > 0 && models.Count > index;
        if (Visible.Value)
        {
            var cellW = 65f;
            var w = cellW * 5f;
            var model = models[index];

            Position.Value = new Vector3(model.X * cellW - w + cellW - 10f, (9 - model.Y) * cellW - w + 10f);
            Rotation.Value = model.Direction == Data.ShipDirection.Horizontal ? -90f : 0f;
        }
    }
}

public class ShipModel
{
    public Data.ShipDirection Direction { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    private static int GetCellHash(int x, int y)
    {
        return x * 1000 + y;
    }

    public static void FillAllShips(Data.FieldCells cells, List<ShipModel> shipsList1, List<ShipModel> shipsList2, List<ShipModel> shipsList3, List<ShipModel> shipsList4)
    {
        var excludedCells = new HashSet<int>();

        shipsList1.Clear();
        shipsList2.Clear();
        shipsList3.Clear();
        shipsList4.Clear();

        FillShips(4, cells, shipsList4, excludedCells);
        FillShips(3, cells, shipsList3, excludedCells);
        FillShips(2, cells, shipsList2, excludedCells);
        FillShips(1, cells, shipsList1, excludedCells);
    }

    public static void FillShips(int shipSize, Data.FieldCells cells, List<ShipModel> shipsList, HashSet<int> excludedCells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            var usedCells = new HashSet<int>();
            var count = 0;
            var x = 0;
            var y = 0;
            for (int j = 0; j < cells[i].Count; j++)
            {
                if (cells[i][j] > 0 && !excludedCells.Contains(GetCellHash(i, j)))
                {
                    if (count == 0)
                    {
                        x = i;
                        y = j;
                    }
                    count++;
                    usedCells.Add(GetCellHash(i, j));
                }
                else
                {
                    count = 0;
                    usedCells.Clear();
                }

                if (count == shipSize)
                {
                    shipsList.Add(new ShipModel
                    {
                        X = x,
                        Y = y,
                        Direction = Data.ShipDirection.Horizontal,
                    });
                    count = 0;

                    foreach (var cell in usedCells) excludedCells.Add(cell);
                    usedCells.Clear();
                }
            }
        }

        for (int i = 0; i < cells.Count; i++)
        {
            var usedCells = new HashSet<int>();
            var count = 0;
            var x = 0;
            var y = 0;
            for (int j = 0; j < cells[i].Count; j++)
            {
                if (cells[j][i] > 0 && !excludedCells.Contains(GetCellHash(j, i)))
                {
                    if (count == 0)
                    {
                        x = j;
                        y = i;
                    }
                    count++;
                    usedCells.Add(GetCellHash(j, i));
                }
                else
                {
                    count = 0;
                    usedCells.Clear();
                }

                if (count == shipSize)
                {
                    shipsList.Add(new ShipModel
                    {
                        X = x,
                        Y = y,
                        Direction = Data.ShipDirection.Vertical,
                    });
                    count = 0;

                    foreach (var cell in usedCells) excludedCells.Add(cell);
                    usedCells.Clear();
                }
            }
        }
    }
}