using System;

namespace AntonioHR.ConditionVariables
{

    [Serializable]
    public class ConditionVariable
    {
        public string name;
        public ConditionVariableValue value;
    }
    [Serializable]
    public class ConditionVariableValue
    {
       public enum Type { Integer, Boolean, Float}

        public ConditionVariableValue()
        {
        }
        public ConditionVariableValue(ConditionVariableValue other)
        {
            type = other.type;
            intValue = other.intValue;
            boolValue = other.boolValue;
            floatValue = other.floatValue;
        }

        public Type type;

        public int intValue;
        public bool boolValue;
        public float floatValue;
    }
}
