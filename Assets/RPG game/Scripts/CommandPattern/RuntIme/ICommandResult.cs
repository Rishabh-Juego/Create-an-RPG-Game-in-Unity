using System;
using JetBrains.Annotations;
using UnityEngine ;


namespace TGL.RPG.CommandPattern
{
    public interface ICommandResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
    }
    
    public interface CommandResult : ICommandResult { }
    
    public interface CommandResult<T> : ICommandResult where T : class
    {
        public T Data { get; }
    }
}