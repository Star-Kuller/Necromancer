using System;

namespace Services.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        
    }
}