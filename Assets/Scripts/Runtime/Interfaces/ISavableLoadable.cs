using UnityEngine;
using System;

namespace Runtime.Interfaces
{
    public interface ISavableLoadable
    {
        void Save();
        void Load();
    }
}