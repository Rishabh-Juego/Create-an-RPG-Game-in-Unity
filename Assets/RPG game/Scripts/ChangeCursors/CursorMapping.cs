using System;
using UnityEngine;

namespace RPG_Game.Scripts.ChangeCursors
{
    [Serializable]
    public struct CursorMapping
    {
        public CursorTypes cursorType;
        public Sprite cursorSprite;
    }
}