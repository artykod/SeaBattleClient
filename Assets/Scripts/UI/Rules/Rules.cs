public class Rules : BindScreen
{
    public Bind<int> Page { get; private set; }
    public Bind<string> TextBack { get; private set; }
    public Bind<string> RulesText { get; private set; }
    public Bind<string> Text1 { get; private set; }
    public Bind<string> Text2 { get; private set; }
    public Bind<string> Text3 { get; private set; }
    public Bind<string> Text4 { get; private set; }
    public Bind<string> Text5 { get; private set; }
    public Bind<string> Text6 { get; private set; }
    public Bind<string> Text7 { get; private set; }
    public Bind<string> Text8 { get; private set; }

    public Rules() : base("Rules/Rules")
    {
        Page.Value = 1;
        TextBack.Value = "В меню";
        RulesText.Value = "Правила";
        Text2.Value = "Размещайте корабли на зеленых клетках. На красных клетках размещать нельзя.";
        Text1.Value = "Выбирайте корабли из списка и перетаскивайте на игровое поле.";
        Text3.Value = "Положение корабля можно менять совершая круговые вращения. Когда размещение корабля окончено, нажмите на него снова и корабль станет темно-синего цвета. Вы также можете вернуть корабль обратно в список.";
        Text5.Value = "Показывает чья очередь ходить.";
        Text4.Value = "Отображает номер хода и положение прицела.";
        Text6.Value = "Ваше поле";
        Text7.Value = "Список кораблей противника. Красным цветом отмечаются подбитые.";
        Text8.Value = "Наводите прицел и стреляйте по клеткам на поле противника. Ваша задача потопить все корабли.";
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void NextPage()
    {
        Page.Value = 2;
    }

    [BindCommand]
    private void PrevPage()
    {
        Page.Value = 1;
    }
}
