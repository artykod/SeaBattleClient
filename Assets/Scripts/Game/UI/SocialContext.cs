using UnityEngine;

public class SocialContext : NeastedBindContext
{
    [BindCommand]
    private void Fb()
    {
        Open("facebook.com");
    }

    [BindCommand]
    private void Vk()
    {
        Open("vk.com");
    }

    [BindCommand]
    private void Ok()
    {
        Open("odnoklassniki.ru");
    }

    [BindCommand]
    private void MyWorld()
    {
        Open("mail.ru");
    }

    [BindCommand]
    private void Twitter()
    {
        Open("twitter.com");
    }

    private void Open(string url)
    {
        Application.OpenURL("https://" + url);
    }
}