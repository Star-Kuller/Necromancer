using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.ServiceLocator
{
    public class ServiceLocator
    {
        #region Singletone

        private ServiceLocator()
        { }
        
        private static ServiceLocator _current = new();

        /// <summary>
        /// Получить текущий сервис
        /// </summary>
        public static ServiceLocator Current
        {
            get => _current ??= new ServiceLocator();
            set => _current = value;
        }

        public static void Reset()
        {
            _current = new ServiceLocator();
        }

        #endregion
        
        private readonly Dictionary<string, IService> _services = new();
        
        /// <summary>
        /// Получить сервис из текущего сервис локатора
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <returns>Сервис</returns>
        /// <exception cref="InvalidOperationException">Не удалось получить сервис</exception>
        public T Get<T>() where T : IService
        {
            var key = typeof(T).Name;
            if (_services.TryGetValue(key, out var service))
                return (T)service;
            
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }
        
        /// <summary>
        /// Попытается получить сервис из текущего сервис локатора
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <returns>Удалось получить сервис</returns>
        public bool TryGet<T>(out T service) where T : IService
        {
            var key = typeof(T).Name;
            var result = _services.TryGetValue(key, out var value);
            service = (T)value;
            return result;
        }

        /// <summary>
        /// Проверяет регистрацию сервиса в текущем сервис локаторе
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <returns>Сервис зарегистрирован</returns>
        public bool IsRegistered<T>() where T : IService
        {
            var key = typeof(T).Name;
            return _services.TryGetValue(key, out var service);
        }

        /// <summary>
        /// Регистрирует сервис в текущем сервис локаторе
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <param name="service">Экземпляр сервиса</param>
        public void Register<T>(T service) where T : IService
        {
            var key = typeof(T).Name;
            
            if (!_services.TryAdd(key, service))
            {
                Debug.LogError(
                    $"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
            }
        }
        
        /// <summary>
        /// Регистрирует сервис в текущем сервис локаторе
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс сервиса</typeparam>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <param name="service">Экземпляр сервиса</param>
        public void Register<TInterface,T>(T service) 
            where TInterface : IService
            where T : class, TInterface
        {
            var key = typeof(TInterface).Name;
            if (!_services.TryAdd(key, service))
            {
                Debug.LogError(
                    $"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
            }
        }
        
        /// <summary>
        /// Пытается зарегистрировать сервис в текущем сервис локаторе
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <param name="service">Экземпляр сервиса</param>
        public bool TryRegister<T>(T service) where T : IService
        {
            var key = typeof(T).Name;
            return _services.TryAdd(key, service);
        }
        
        /// <summary>
        /// Пытается зарегистрировать сервис в текущем сервис локаторе
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс сервиса</typeparam>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <param name="service">Экземпляр сервиса</param>
        public bool TryRegister<TInterface,T>(T service)
            where TInterface : IService
            where T : class, TInterface
        {
            var key = typeof(TInterface).Name;
            return _services.TryAdd(key, service);
        }

        /// <summary>
        /// Убирает сервис из текущего сервис локатора
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        public void Unregister<T>() where T : IService
        {
            var key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            _services.Remove(key);
        }
    }
}