using System;
using UnityEngine ;

namespace TGL.RPG.CommandPattern
{
    public interface ICommand<T> where T : class
    {
        CommandResult<T> Execute();
        bool Undo();
    }
}
