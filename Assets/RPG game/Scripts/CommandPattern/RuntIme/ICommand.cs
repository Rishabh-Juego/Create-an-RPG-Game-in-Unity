using System;
using UnityEngine ;

namespace TGL.RPG.CommandPattern
{
    public interface ICommand { }

    public interface Command : ICommand
    {
        ICommandResult Execute();
        bool Undo();
    }
    
    public interface ICommand<T> : ICommand where T : class 
    {
        T Data { get; }
        CommandResult Execute();
        bool Undo();
    }
    
    public interface ICommand<T, V> : ICommand where T : class where V : class
    {
        T Data { get; }
        CommandResult<V> Execute();
        bool Undo();
    }
}
