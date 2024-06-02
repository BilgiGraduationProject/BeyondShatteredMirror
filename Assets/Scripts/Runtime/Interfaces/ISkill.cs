using Runtime.Enums;
using UnityEngine;

namespace Runtime.Interfaces
{
    public interface ISkill
    {
        void Activate();
        void Deactivate();
        float Duration { get; }
        bool IsActive { get; }
        public PillTypes PillType { get; }
    }
}

