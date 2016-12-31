using Data;

public class FieldDiffTool
{
    private static bool IsValidData(FieldStateData prev, FieldStateData curr)
    {
        return prev != null && curr != null;
    }

    public static bool CheckKillShip(FieldStateData prev, FieldStateData curr)
    {
        if (!IsValidData(prev, curr)) return false;

        return 
            prev.AliveShips.Count1 != curr.AliveShips.Count1 ||
            prev.AliveShips.Count2 != curr.AliveShips.Count2 ||
            prev.AliveShips.Count3 != curr.AliveShips.Count3 ||
            prev.AliveShips.Count4 != curr.AliveShips.Count4;
    }

    public static bool CheckHit(FieldStateData prev, FieldStateData curr)
    {
        if (!IsValidData(prev, curr)) return false;

        var prevHitsCount = 0;
        var currHitsCount = 0;

        foreach (var r in prev.Field.Cells)
            foreach (var c in r)
                if (c == 3 || c == 4) prevHitsCount++;

        foreach (var r in curr.Field.Cells)
            foreach (var c in r)
                if (c == 3 || c == 4) currHitsCount++;

        return prevHitsCount != currHitsCount;
    }

    public static bool CheckMiss(FieldStateData prev, FieldStateData curr)
    {
        if (!IsValidData(prev, curr)) return false;

        var prevMissCount = 0;
        var currMissCount = 0;

        foreach (var r in prev.Field.Cells)
            foreach (var c in r)
                if (c == 2) prevMissCount++;

        foreach (var r in curr.Field.Cells)
            foreach (var c in r)
                if (c == 2) currMissCount++;

        return prevMissCount != currMissCount;
    }
}
