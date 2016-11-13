public class YesNoDialog : DialogBase
{
    public class ButtonContext : NeastedBindContext
    {
        public YesNoDialog Dialog;

        public Bind<string> Text;
        public Bind<bool> IsVisible;
        public System.Action Callback;

        [BindCommand]
        private void Click()
        {
            Dialog.Exit();
            if (Callback != null) Callback();
        }
    }

    public Bind<string> Title;
    public ButtonContext Button1;
    public ButtonContext Button2;

    private int _buttonIndex;

    public YesNoDialog AddButton(string text, System.Action callback)
    {
        var btn = default(ButtonContext);
        if (_buttonIndex < 1) btn = Button1; else
        if (_buttonIndex < 2) btn = Button2;
        _buttonIndex++;
        if (btn == null) return this;

        btn.Dialog = this;
        btn.Text.Value = LanguageController.Localize(text);
        btn.IsVisible.Value = true;
        btn.Callback = callback;

        return this;
    }

    public YesNoDialog(string title) : base("YesNoDialog")
    {
        Title.Value = LanguageController.Localize(title);
        SoundController.Sound(SoundController.SOUND_BEEP);
    }
}
