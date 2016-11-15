using UnityEngine;
using System.Collections.Generic;

public class FieldShipsAliveStateContext : NeastedBindContext
{
    public Bind<int> Ship1_1;
    public Bind<int> Ship1_2;
    public Bind<int> Ship1_3;
    public Bind<int> Ship1_4;
    public Bind<int> Ship2_1;
    public Bind<int> Ship2_2;
    public Bind<int> Ship2_3;
    public Bind<int> Ship3_1;
    public Bind<int> Ship3_2;
    public Bind<int> Ship4_1;
}

public class FieldCellContext : NeastedBindContext
{
    public Bind<int> Mode;
}

public class FieldCellsLineContext : NeastedBindContext
{
    public FieldCellContext Cell_1;
    public FieldCellContext Cell_2;
    public FieldCellContext Cell_3;
    public FieldCellContext Cell_4;
    public FieldCellContext Cell_5;
    public FieldCellContext Cell_6;
    public FieldCellContext Cell_7;
    public FieldCellContext Cell_8;
    public FieldCellContext Cell_9;
    public FieldCellContext Cell_10;
}

public class FieldContext : NeastedBindContext
{
    public static void FillCells(Data.FieldCellsData cells, params FieldCellsLineContext[] lines)
    {
        for (int i = 0; i < cells.Cells.Count; i++)
        {
            lines[i].Cell_1.Mode.Value  = cells.Cells[0][i];
            lines[i].Cell_2.Mode.Value  = cells.Cells[1][i];
            lines[i].Cell_3.Mode.Value  = cells.Cells[2][i];
            lines[i].Cell_4.Mode.Value  = cells.Cells[3][i];
            lines[i].Cell_5.Mode.Value  = cells.Cells[4][i];
            lines[i].Cell_6.Mode.Value  = cells.Cells[5][i];
            lines[i].Cell_7.Mode.Value  = cells.Cells[6][i];
            lines[i].Cell_8.Mode.Value  = cells.Cells[7][i];
            lines[i].Cell_9.Mode.Value  = cells.Cells[8][i];
            lines[i].Cell_10.Mode.Value = cells.Cells[9][i];
        }
    }
}

public class ShipCountViewContext : NeastedBindContext
{
    public Bind<int> Mode;
    public Bind<int> Count;

    public ShipCountViewContext()
    {
        //Count.OnValueChanged += (val) => Mode.Value = val.Value > 0 ? 0 : 2;
        Mode.Value = 2;
    }
}

public class ShipViewContext : NeastedBindContext
{
    public Bind<bool> Visible;
    public Bind<float> Rotation;
    public Bind<Vector3> Position;
    public Bind<GameObject> Object;
    public Bind<GameObject> ViewOnField;

    private Vector3 _initialPosition;

    public ShipModel Model { get; private set; }

    public ShipViewContext()
    {
        Position.Value = new Vector3(0f, 9999f);
    }

    public void FetchFromModel(List<ShipModel> models, int index)
    {
        if (Object.Value != null) _initialPosition = Object.Value.transform.localPosition;

        Visible.Value = models.Count > 0 && models.Count > index;
        if (Visible.Value)
        {
            var cellW = 64f;
            var w = cellW * 5f;
            Model = models[index];
            UpdateData(new Vector3(Model.X * cellW - w + cellW - 10f, (9 - Model.Y) * cellW - w + 10f), Model.Direction);
        }
        else
        {
            Reset();
        }

        if (ViewOnField.Value != null)
        {
            var layoutShip = ViewOnField.Value.GetComponent<LayoutShip>();
            if (layoutShip) layoutShip.FetchShipView(this);
        }
    }

    public void Reset()
    {
        Model = null;
        UpdateData(_initialPosition, Data.ShipDirection.Horizontal);
    }

    private void UpdateData(Vector3 position, Data.ShipDirection direction)
    {
        Position.Value = position;
        Rotation.Value = direction == Data.ShipDirection.Vertical ? -90f : 0f;
    }
}

public class ShipModel
{
    public Data.ShipDirection Direction { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    private static int GetCellHash(int x, int y)
    {
        return x * 1000 + y;
    }

    public static void FillAllShips(bool showWounded, Data.FieldCellsData cells, List<ShipModel> shipsList1, List<ShipModel> shipsList2, List<ShipModel> shipsList3, List<ShipModel> shipsList4)
    {
        var excludedCells = new HashSet<int>();

        shipsList1.Clear();
        shipsList2.Clear();
        shipsList3.Clear();
        shipsList4.Clear();

        FillShips(showWounded, 4, cells, shipsList4, excludedCells);
        FillShips(showWounded, 3, cells, shipsList3, excludedCells);
        FillShips(showWounded, 2, cells, shipsList2, excludedCells);
        FillShips(showWounded, 1, cells, shipsList1, excludedCells);
    }

    public static void FillShips(bool showWounded, int shipSize, Data.FieldCellsData cells, List<ShipModel> shipsList, HashSet<int> excludedCells)
    {
        for (int i = 0; i < cells.Cells.Count; i++)
        {
            var usedCells = new HashSet<int>();
            var count = 0;
            var x = 0;
            var y = 0;
            for (int j = 0; j < cells.Cells[i].Count; j++)
            {
                if ((cells.Cells[i][j] == 1 || (showWounded && cells.Cells[i][j] == 3) || cells.Cells[i][j] == 4) && !excludedCells.Contains(GetCellHash(i, j)))
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
                        Direction = Data.ShipDirection.Vertical,
                    });
                    count = 0;

                    foreach (var cell in usedCells) excludedCells.Add(cell);
                    usedCells.Clear();
                }
            }
        }

        for (int i = 0; i < cells.Cells.Count; i++)
        {
            var usedCells = new HashSet<int>();
            var count = 0;
            var x = 0;
            var y = 0;
            for (int j = 0; j < cells.Cells[i].Count; j++)
            {
                if ((cells.Cells[j][i] == 1 || (showWounded && cells.Cells[j][i] == 3) || cells.Cells[j][i] == 4) && !excludedCells.Contains(GetCellHash(j, i)))
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
                        Direction = Data.ShipDirection.Horizontal,
                    });
                    count = 0;

                    foreach (var cell in usedCells) excludedCells.Add(cell);
                    usedCells.Clear();
                }
            }
        }
    }
}