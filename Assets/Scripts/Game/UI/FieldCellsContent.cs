public class FieldCellsContent : BindModel
{
    public FieldCellsLineContext Line_1;
    public FieldCellsLineContext Line_2;
    public FieldCellsLineContext Line_3;
    public FieldCellsLineContext Line_4;
    public FieldCellsLineContext Line_5;
    public FieldCellsLineContext Line_6;
    public FieldCellsLineContext Line_7;
    public FieldCellsLineContext Line_8;
    public FieldCellsLineContext Line_9;
    public FieldCellsLineContext Line_10;

    public FieldCellsContent() : base("UI/FieldCells")
    {
    }

    public void UpdateData(Data.FieldCellsData cells)
    {
        FieldContext.FillCells(cells, Line_1, Line_2, Line_3, Line_4, Line_5, Line_6, Line_7, Line_8, Line_9, Line_10);
    }
}
