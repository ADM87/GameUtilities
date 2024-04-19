using System;
using System.Collections.Generic;
using System.Reflection;

namespace ADM87.GameUtilities.ServiceProvider
{
    internal class ServiceDefinition
    {
        public Type Identity                            { get; internal set; }
        public Type Implementation                      { get; internal set; }
        public IEnumerable<PropertyInfo> Dependencies   { get; internal set; }
        public bool IsSingleton                         { get; internal set; }
        public object SingletonInstance                 { get; internal set; }
    }
}
