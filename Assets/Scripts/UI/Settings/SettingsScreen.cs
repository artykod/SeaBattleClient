public class SettingsScreen : BindScreen
{
    public Bind<int> Language;
    public Bind<string> TextBack;
    public Bind<float> Volume;
    public Bind<bool> IsVolumeDisabled;
    public Bind<string> VolumeText;
    public Bind<string> LanguageText;

    public SettingsScreen() : base("Settings/Settings")
    {
        Language.Value = 1;
        TextBack.Value = "В меню";
        Volume.OnValueChanged += (val) => IsVolumeDisabled.Value = val.Value < 0.0001f;
        Volume.Value = 0.5f;
        VolumeText.Value = "Звук";
        LanguageText.Value = "Язык/Language";
    }

    [BindCommand]
    private void ApplyFirstLanguage()
    {
        Language.Value = 1;
    }

    [BindCommand]
    private void ApplySecondLanguage()
    {
        Language.Value = 2;
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }
}
