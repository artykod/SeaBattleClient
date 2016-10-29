public class Settings : EmptyScreenWithBackground
{
    public Bind<int> Language;
    public Bind<float> Volume;
    public Bind<bool> IsVolumeDisabled;

    public Settings() : base("Settings")
    {
        Language.Value = LanguageController.Instance.CurrentLanguage == global::Language.Russian ? 1 : 2;
        IsVolumeDisabled.Value = !SoundController.IsSoundEnabled;
        Volume.OnValueChanged += (val) =>
        {
            SoundController.SoundVolume = val.Value;
            SoundController.IsSoundEnabled = val.Value > 0.0001f;
            IsVolumeDisabled.Value = !SoundController.IsSoundEnabled;
        };
        Volume.Value = SoundController.IsSoundEnabled ? SoundController.SoundVolume : 0f;
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
