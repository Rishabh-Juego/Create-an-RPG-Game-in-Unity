
using System.Collections.Generic;

namespace TGL.RPG.CommandPattern
{
    public interface ICommandManager<T> where T : class
    {
        List<ICommand> CommandHistory { get; }
        void AddCommand(ICommand command);
    }
}