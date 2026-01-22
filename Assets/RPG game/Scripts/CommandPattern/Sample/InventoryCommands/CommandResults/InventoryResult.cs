using System;
using JetBrains.Annotations;
using UnityEngine ;

namespace TGL.RPG.CommandPattern
{
    public class InventoryResult : CommandResult
    {
        private bool _isSuccess;
        private string _message;
        
        public bool IsSuccess => _isSuccess;
        public string Message => _message;
        
    
        public InventoryResult(bool isSuccess, string message)
        {
            _isSuccess = isSuccess;
            _message = message;
        }
    
        public InventoryResult(string message)
        {
            _isSuccess = false;
            _message = message;
        }
    
        public InventoryResult(bool isSuccess)
        {
            _isSuccess = isSuccess;
            _message = string.Empty;
        }
    }

    public class InventoryResult<T> : CommandResult<T>  where T : class
    {
        private bool _isSuccess;
        private string _message;
        public T Data { get; }
    
        public bool IsSuccess => _isSuccess;
        public string Message => _message;
    
        public InventoryResult(bool isSuccess, string message, T data)
        {
            _isSuccess = isSuccess;
            _message = message;
            Data = data;
        }
    
        public InventoryResult(string message, T data)
        {
            _isSuccess = false;
            _message = message;
            Data = data;
        }
    
        public InventoryResult(bool isSuccess, T data)
        {
            _isSuccess = isSuccess;
            _message = string.Empty;
            Data = data;
        }
    }
}
