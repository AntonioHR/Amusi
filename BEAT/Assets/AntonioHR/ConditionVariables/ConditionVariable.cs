using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.ConditionVariables
{
    [Serializable]
    public class ConditionVariable
    {
       public enum Type { Integer, Boolean, Float}

        public Type type;

        public int intValue;
        public bool boolValue;
        public float floatValue;
    }
}
