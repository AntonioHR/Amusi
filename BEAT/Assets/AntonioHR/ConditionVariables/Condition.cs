using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.ConditionVariables
{
    public class Condition
    {
        public enum IntCondition { EqualTo, Greater, GreaterEq, Less, LessEq}
        public enum FloatCondition { EqualTo, Greater, GreaterEq, Less, LessEq }
        public enum BooleanCondition { Is }

        public IntCondition intCondition;
        public FloatCondition floatCondition;
        public BooleanCondition booleanCondition;
        public int intVal;
        public float floatVal;
        public bool boolVal;

        public string variableName;

        public bool IsTrueFor(int i)
        {
            switch (intCondition)
            {
                case IntCondition.EqualTo:
                    return i == intVal;
                case IntCondition.Greater:
                    return i > intVal;
                case IntCondition.GreaterEq:
                    return i >= intVal;
                case IntCondition.Less:
                    return i < intVal;
                case IntCondition.LessEq:
                    return i <= intVal;
                default:
                    return false;
            }
        }
        public bool IsTrueFor(float f)
        {
            switch (floatCondition)
            {
                case FloatCondition.EqualTo:
                    return f == floatVal;
                case FloatCondition.Greater:
                    return f > floatVal;
                case FloatCondition.GreaterEq:
                    return f >= floatVal;
                case FloatCondition.Less:
                    return f < floatVal;
                case FloatCondition.LessEq:
                    return f <= floatVal;
                default:
                    return false;;
            }
        }
        public bool IsTrueFor(bool b)
        {
            switch (booleanCondition)
            {
                case BooleanCondition.Is:
                    return boolVal == b;
                default:
                    return false;
            }
        }
    }
}
