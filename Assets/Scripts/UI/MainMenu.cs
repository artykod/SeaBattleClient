using System;
using System.Linq.Expressions;
using UnityEngine;

public class MainMenu : BindingBehaviour
{


    private string _bindedText;
    public string BindedText
    {
        get
        {
            return _bindedText;
        }
        set
        {
            Set(ref _bindedText, value, () => BindedText);
        }
    }

    private string _buttonText;
    public string ButtonText
    {
        get
        {
            return _buttonText;
        }
        set
        {
            Set(ref _buttonText, value, () => ButtonText);
        }
    }

    private void Start()
    {
        BindedText = "Hello world";
        ButtonText = "Click me";
    }

    private static int counter;

    public void StartGame()
    {
        ButtonText = (counter++).ToString();
    }
}
