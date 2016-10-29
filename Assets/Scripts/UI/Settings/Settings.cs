public class Settings : BindScreen
{
    public Bind<int> Language;
    public Bind<float> Volume;
    public Bind<bool> IsVolumeDisabled;

    public Settings() : base("Settings/Settings")
    {
        Language.Value = LanguageController.Instance.CurrentLanguage == global::Language.Russian ? 1 : 2;
        Volume.OnValueChanged += (val) => IsVolumeDisabled.Value = val.Value < 0.0001f;
        Volume.Value = 0.5f;
    }

    [BindCommand]
    private void ApplyFirstLanguage()
    {
        Language.Value = 1;
        LanguageController.Instance.CurrentLanguage = global::Language.Russian;
    }

    [BindCommand]
    private void ApplySecondLanguage()
    {
        Language.Value = 2;
        LanguageController.Instance.CurrentLanguage = global::Language.English;
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }
}
