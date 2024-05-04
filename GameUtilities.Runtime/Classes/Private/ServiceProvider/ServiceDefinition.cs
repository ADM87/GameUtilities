using System;
using System.Collections.Generic;
using System.Reflection;
using ADM87.GameUtilities.Services;

namespace ADM87.GameUtilities.Services
{
    internal class ServiceDefinition
    {
        public Type Identity                            { get; internal set; }
        public Type Implementation                      { get; internal set; }
        public IEnumerable<PropertyInfo> Dependencies   { get; internal set; }
        public EServiceLifeTime ServiceLifeTime      { get; internal set; }
        public object Instance                          { get; internal set; }
    }
}
