public class Settings : EmptyScreenWithBackground
{
    public Bind<int> Language;
    public Bind<float> Volume;

    public Settings() : base("Settings")
    {
        Language.Value = LanguageController.Instance.CurrentLanguage == global::Language.Russian ? 1 : 2;
        Volume.OnValueChanged += val =>
        {
            if (val.Value > 0.0001f)
            {
                SoundController.SoundVolume = val.Value;
                SoundController.IsSoundEnabled = true;
            }
            else
            {
                SoundController.IsSoundEnabled = false;
            }
            IsSoundEnabled.Value = SoundController.IsSoundEnabled;
        };
        Volume.Value = SoundController.IsSoundEnabled ? SoundController.SoundVolume : 0f;
        IsSoundEnabled.OnValueChanged += val => Volume.Value = SoundController.IsSoundEnabled ? SoundController.SoundVolume : 0f;
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
        Core.Instance.SaveSettings();
        Destroy();
    }
}
