using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.ConditionVariables
{
    [Serializable]
    public class ConditionVariableCollection
    {
        public List<NamedConditionVariable> variables;
    }
    [Serializable]
    public class NamedConditionVariable
    {
        public string name;
        public ConditionVariable value;
    }
}
