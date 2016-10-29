public class Settings : BindScreen
{
    public Bind<int> Language;
    public Bind<float> Volume;
    public Bind<bool> IsVolumeDisabled;

    public Settings() : base("Settings/Settings")
    {
        Language.Value = LanguageController.Instance.CurrentLanguage == global::Language.Russian ? 1 : 2;
        IsVolumeDisabled.Value = !SoundController.Instance.IsSoundEnabled;
        Volume.OnValueChanged += (val) =>
        {
            SoundController.Instance.SoundVolume = val.Value;
            SoundController.Instance.IsSoundEnabled = val.Value > 0.0001f;
            IsVolumeDisabled.Value = !SoundController.Instance.IsSoundEnabled;
        };
        Volume.Value = SoundController.Instance.IsSoundEnabled ? SoundController.Instance.SoundVolume : 0f;
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
