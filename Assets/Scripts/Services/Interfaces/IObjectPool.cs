using Services.ServiceLocator;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IObjectPool : IService
    {
        void Destroy(string key, GameObject @object);
        GameObject Create(string key, GameObject target);
        GameObject Create(string key, GameObject target, Transform parent);
    }
}