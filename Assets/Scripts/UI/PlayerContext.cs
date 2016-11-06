using UnityEngine;

public class PlayerContext : NeastedBindContext
{
    public Bind<Texture2D> Avatar;
    public Bind<string> Name;
    public Bind<int> Gold;
    public Bind<int> Silver;
}