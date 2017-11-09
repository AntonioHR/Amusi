using AntonioHR.ConditionVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.Amusi.Playback
{
    public class MusicTreeEnvironment
    {
        private Dictionary<string, ConditionVariableValue> values;

        private MusicTreeEnvironment()
        {
            values = new Dictionary<string, ConditionVariableValue>();
        }

        public bool Evaluate(Condition cond)
        {

            var result = cond.IsTrueFor(values[cond.variableName]);

            UnityEngine.Debug.LogFormat("Condition for variable {0}  was evaluated {1}", cond.variableName, result);
            return result;
        }

        public float GetFloatValue(string name)
        {
            var val = values[name];
            if (val.type != ConditionVariableValue.Type.Float)
            {
                throw new WrongVariableTypeException(name, ConditionVariableValue.Type.Float, val.type);
            }
            return val.floatValue;
        }
        public void SetFloatValue(string name, float newVal)
        {
            var val = values[name];
            if(val.type != ConditionVariableValue.Type.Float)
            {
                throw new WrongVariableTypeException(name, ConditionVariableValue.Type.Float, val.type);
            }
            val.floatValue = newVal;
        }

        public void SetBoolValue(string name, bool newVal)
        {
            var val = values[name];
            if (val.type != ConditionVariableValue.Type.Boolean)
            {
                throw new WrongVariableTypeException(name, ConditionVariableValue.Type.Boolean, val.type);
            }
            val.boolValue = newVal;
        }
        public bool GetBoolValue(string name)
        {
            var val = values[name];
            if (val.type != ConditionVariableValue.Type.Boolean)
            {
                throw new WrongVariableTypeException(name, ConditionVariableValue.Type.Boolean, val.type);
            }
            return val.boolValue;
        }

        public void SetIntValue(string name, int newVal)
        {
            var val = values[name];
            if (val.type != ConditionVariableValue.Type.Integer)
            {
                throw new WrongVariableTypeException(name, ConditionVariableValue.Type.Integer, val.type);
            }
            val.intValue = newVal;
        }
        public int GetIntValue(string name)
        {
            var val = values[name];
            if (val.type != ConditionVariableValue.Type.Integer)
            {
                throw new WrongVariableTypeException(name, ConditionVariableValue.Type.Integer, val.type);
            }
            return val.intValue;
        }


        public static MusicTreeEnvironment CreateFrom(IEnumerable<ConditionVariable> varDescriptions)
        {
            var result = new MusicTreeEnvironment();

            foreach (var varDescription in varDescriptions)
            {
                result.values.Add(varDescription.name, new ConditionVariableValue(varDescription.value));
            }

            return result;
        }
    }
    public class WrongVariableTypeException: Exception
    {
        public ConditionVariableValue.Type ExpectedType{get;private set;}
        public ConditionVariableValue.Type ActualType{get;private set;}
        public string VarName{ get; private set; }

        public WrongVariableTypeException(string varName, ConditionVariableValue.Type expectedType, ConditionVariableValue.Type actualType)
        {
            this.ExpectedType = expectedType;
            this.ActualType = actualType;
            this.VarName = varName;
        }
        public override string Message
        {
            get
            {
                return string.Format("Tried to use {0} as a {0}, but it is a {1}", VarName, ExpectedType, ActualType);
            }
        }
    }
}
