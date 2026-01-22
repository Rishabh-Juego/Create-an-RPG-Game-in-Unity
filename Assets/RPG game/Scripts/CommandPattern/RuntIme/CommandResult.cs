using System;
using UnityEngine ;

namespace TGL.RPG.CommandPattern
{
    public struct CommandResult<T> where T : class
    {
        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }
    
        public CommandResult(bool success, string message = "", T data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}