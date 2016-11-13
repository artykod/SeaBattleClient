public class SocialContext : NeastedBindContext
{
    [BindCommand]
    private void Fb()
    {
        Open(GameConfig.Instance.Config.FacebookUrl);
    }

    [BindCommand]
    private void Vk()
    {
        Open(GameConfig.Instance.Config.VkUrl);
    }

    [BindCommand]
    private void Ok()
    {
        Open(GameConfig.Instance.Config.OdnoklassnikiUrl);
    }

    [BindCommand]
    private void MyWorld()
    {
        Open(GameConfig.Instance.Config.MyMailUrl);
    }

    [BindCommand]
    private void Twitter()
    {
        Open(GameConfig.Instance.Config.TwitterUrl);
    }

    private void Open(string url)
    {
        Core.OpenUrl(url);
    }
}