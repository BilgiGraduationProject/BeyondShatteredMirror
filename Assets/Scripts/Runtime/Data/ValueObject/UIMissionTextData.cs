using System;
using Runtime.Enums.UI;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct UIMissionTextData
    {
        public UITextEnum uiTextEnum;
        public string text;
    }
}