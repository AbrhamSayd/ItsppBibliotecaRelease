﻿using System;
using System.Windows.Input;

namespace WPFBiblioteca.ViewModels;

public class ViewModelCommand : ICommand
{
    private readonly Predicate<object> _canExecuteAction;

    //Fields
    private readonly Action<object> _executeAction;

    //Constructors
    public ViewModelCommand(Action<object> executeAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = null;
    }

    public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = canExecuteAction;
    }

    //Events
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    //Methods
    public bool CanExecute(object parameter)
    {
        return _canExecuteAction == null ? true : _canExecuteAction(parameter);
    }

    public void Execute(object parameter)
    {
        _executeAction(parameter);
    }
}