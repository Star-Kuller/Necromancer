using UnityEngine;

namespace Services.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static T Inject<T>(this T obj)
        {
            DependencyInjector.Current.Inject(obj);
            return obj;
        }
        
        public static GameObject Inject(this GameObject obj)
        {
            DependencyInjector.Current.Inject(obj);
            return obj;
        }
        
        public static T Register<T>(this T obj)
        {
            DependencyInjector.Current.Register(obj);
            return obj;
        }
        
        public static GameObject Register(this GameObject obj)
        {
            DependencyInjector.Register(obj);
            return obj;
        }
    }
}