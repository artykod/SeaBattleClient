public class GameObjectBinding : BindViewMonoBehaviour
{
    protected override void OnBind()
    {
        base.OnBind();
        CommitValue();
    }

    protected override object GetValueHandler()
    {
        return gameObject;
    }

    protected override void ValueChangedHandler(object value)
    {
    }
}