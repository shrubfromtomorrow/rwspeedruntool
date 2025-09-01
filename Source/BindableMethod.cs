using System;

namespace SpeedrunTool
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BindableMethod : Attribute
    {
        public string name;
        public string category;
    }
}