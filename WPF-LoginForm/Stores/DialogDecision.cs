using System;

namespace WPFBiblioteca.Stores;

public class DialogDecision
{
    public event Action<string> DecisionTaked;

    public void CreateDecision(string decision)
    {
        DecisionTaked?.Invoke(decision);
    }
}