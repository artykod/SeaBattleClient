using UnityEngine;

public class Background : BindModel
{
    public Bind<Texture2D> Image;

    public Background() : base("Screens/Background")
    {
        Image.Value = Resources.Load<Texture2D>("Textures/bg");
    }
}