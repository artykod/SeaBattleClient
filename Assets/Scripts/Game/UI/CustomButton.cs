using UnityEngine.UI;

public class CustomButton : Button
{
    public enum SelectionStatePublic
    {
        Normal = 0,
        Highlighted = 1,
        Pressed = 2,
        Disabled = 3
    }

    public event System.Action<SelectionStatePublic> onStateChanged = delegate { };

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
        onStateChanged((SelectionStatePublic)state);
    }
}
