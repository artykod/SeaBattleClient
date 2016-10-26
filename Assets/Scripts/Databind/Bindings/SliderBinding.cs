using UnityEngine.UI;

public class SliderBinding : BindViewMonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(ValueChanged);
    }

    protected override object GetValueHandler()
    {
        return _slider.normalizedValue;
    }

    protected override void ValueChangedHandler(object value)
    {
        _slider.normalizedValue = (float)System.Convert.ChangeType(value, typeof(float));
    }

    private void ValueChanged(float value)
    {
        CommitValue();
    }
}
