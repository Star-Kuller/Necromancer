using JetBrains.Annotations;
using Services.DependencyInjection;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IObjectPool : IInjectable
    {
        void Destroy(string key, GameObject target);
        GameObject Create(string key, GameObject target, [CanBeNull] Transform parent = default, bool isActive = true);
    }
}