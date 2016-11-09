using UnityEngine;
using System.Collections.Generic;

public class ShipCountViewContext : NeastedBindContext
{
    public Bind<int> Count { get; set; }
    public Bind<int> Mode { get; set; }

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
    public Bind<Vector3> Position;
    public Bind<float> Rotation;

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
    public int X { get; private set; }
    public int Y { get; private set; }
    public Data.ShipDirection Direction { get; private set; }

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

public class Layout : EmptyScreenWithBackground
{
    public ShipCountViewContext Ship1 { get; private set; }
    public ShipCountViewContext Ship2 { get; private set; }
    public ShipCountViewContext Ship3 { get; private set; }
    public ShipCountViewContext Ship4 { get; private set; }

    public ShipViewContext FieldShip1_1 { get; private set; }
    public ShipViewContext FieldShip1_2 { get; private set; }
    public ShipViewContext FieldShip1_3 { get; private set; }
    public ShipViewContext FieldShip1_4 { get; private set; }
    public ShipViewContext FieldShip2_1 { get; private set; }
    public ShipViewContext FieldShip2_2 { get; private set; }
    public ShipViewContext FieldShip2_3 { get; private set; }
    public ShipViewContext FieldShip3_1 { get; private set; }
    public ShipViewContext FieldShip3_2 { get; private set; }
    public ShipViewContext FieldShip4_1 { get; private set; }

    private List<ShipModel> _ships_1 = new List<ShipModel>();
    private List<ShipModel> _ships_2 = new List<ShipModel>();
    private List<ShipModel> _ships_3 = new List<ShipModel>();
    private List<ShipModel> _ships_4 = new List<ShipModel>();

    public Layout() : base("Layout")
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        Core.Instance.Match.ResetField();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
    }

    private void OnMatchReceived(Data.Match match)
    {
        ShipModel.FillAllShips(match.My.Field, _ships_1, _ships_2, _ships_3, _ships_4);

        FieldShip1_1.FetchFromModel(_ships_1, 0);
        FieldShip1_2.FetchFromModel(_ships_1, 1);
        FieldShip1_3.FetchFromModel(_ships_1, 2);
        FieldShip1_4.FetchFromModel(_ships_1, 3);
        FieldShip2_1.FetchFromModel(_ships_2, 0);
        FieldShip2_2.FetchFromModel(_ships_2, 1);
        FieldShip2_3.FetchFromModel(_ships_2, 2);
        FieldShip3_1.FetchFromModel(_ships_3, 0);
        FieldShip3_2.FetchFromModel(_ships_3, 1);
        FieldShip4_1.FetchFromModel(_ships_4, 0);

        Ship1.Count.Value = match.My.FreeShips.Count1;
        Ship2.Count.Value = match.My.FreeShips.Count2;
        Ship3.Count.Value = match.My.FreeShips.Count3;
        Ship4.Count.Value = match.My.FreeShips.Count4;
    }

    [BindCommand]
    private void ResetLayout()
    {
        Core.Instance.Match.ResetField();
    }

    [BindCommand]
    private void AutoLayout()
    {
        Core.Instance.Match.AutolayoutField();
    }

    [BindCommand]
    private void Battle()
    {
        new Game();
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void Help()
    {
        new Rules();
    }
}