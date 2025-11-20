using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Models;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class ObjectPool : MonoBehaviour, IObjectPool, IAutoRegistration
    {
        private readonly Dictionary<string, List<GameObject>> _pool = new();
        
        public void Register()
        {
            this.Register<IObjectPool>();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Destroy(string key, GameObject target)
        {
            target.SetActive(false);
            target.transform.position = Vector3.zero;
            var resetables = target.GetComponents<IResetable>();
            foreach (var resetable in resetables)
            {
                resetable.Reset();
            }
            target.transform.SetParent(transform);
            if (_pool.TryGetValue(key, out var value))
            {
                value.Add(target);
            }
            else
            {
                _pool.Add(key, new List<GameObject>() { target });
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public GameObject Create(string key, GameObject prefab, Transform parent = null, bool isActive = true)
        {
            if (!_pool.TryGetValue(key, out var list)) return Create(prefab, parent, isActive);
            
            var obj = list.FirstOrDefault();
            if (obj is null)
                return Create(prefab, parent, isActive);
            
            list.Remove(obj);
            obj.transform.SetParent(parent);
            obj.SetActive(isActive);
            return obj;
        }

        private static GameObject Create(GameObject prefab, [CanBeNull] Transform parent = null, bool isActive = true)
        {
            var prefabActive = prefab.activeSelf;
            prefab.SetActive(false);
            var obj = Instantiate(prefab, parent);
            obj.Inject();
            prefab.SetActive(prefabActive);
            obj.SetActive(isActive);
            return obj;
        }
    }
}