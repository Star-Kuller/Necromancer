using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class ObjectPool : MonoBehaviour, IObjectPool
    {

        private Dictionary<string, List<GameObject>> _pool = new();

        private void Awake()
        {
            var services = ServiceLocator.ServiceLocator.Current;
            services.TryRegister<IObjectPool>(this);
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
            if (_pool.ContainsKey(key))
            {
                _pool[key].Add(target);
            }
            else
            {
                _pool.Add(key, new List<GameObject>() { target });
            }
        }

        public GameObject Create(string key, GameObject target)
        {
            if (!_pool.TryGetValue(key, out var list)) return Instantiate(target);
            
            var obj = list.FirstOrDefault();
            if (obj is null) return Instantiate(target);
                
            list.Remove(obj);
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;

        }
        
        public GameObject Create(string key, GameObject target, Transform parent)
        {
            if (!_pool.TryGetValue(key, out var list)) return Instantiate(target, parent);
            
            var obj = list.FirstOrDefault();
            if (obj is null) return Instantiate(target, parent);
                
            list.Remove(obj);
            obj.transform.SetParent(parent);
            obj.SetActive(true);
            return obj;

        }
    }
}