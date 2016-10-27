using UnityEngine;

public class PlayerContext : BindContext
{
    public Bind<Texture2D> Avatar;
    public Bind<string> Name;
    public Bind<int> Gold;
    public Bind<int> Silver;
}