using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.DependencyInjection
{
    public class DependencyInjector
    {
        #region Container life cicle

        private DependencyInjector()
        { }
        
        private static DependencyInjector _current = new();

        /// <summary>
        /// Получить текущий контейнер
        /// </summary>
        public static DependencyInjector Current => _current ?? new DependencyInjector();
        
        /// <summary>
        /// Сбросить все зависимости в контейнере
        /// </summary>
        public static void Reset()
        {
            _current = new DependencyInjector();
        }
        #endregion

        #region AutoInit

        public void AutoRegisterAndInject(Scene scene)
        {
            var allObjects = scene.GetRootGameObjects();
            foreach (var rootObject in allObjects)
            {
                RegisterProcessGameObjectHierarchy(rootObject);
            }
            foreach (var rootObject in allObjects)
            {
                InjectProcessGameObjectHierarchy(rootObject);
            }
        }
        
        private void RegisterProcessGameObjectHierarchy(GameObject obj)
        {
            Register(obj);
            
            foreach (Transform child in obj.transform)
            {
                RegisterProcessGameObjectHierarchy(child.gameObject);
            }
        }

        private void InjectProcessGameObjectHierarchy(GameObject obj)
        {
            Inject(obj);
            
            foreach (Transform child in obj.transform)
            {
                InjectProcessGameObjectHierarchy(child.gameObject);
            }
        }

        #endregion
        
        private readonly Dictionary<Type, Func<object>> _container = new();
        
        
        /// <summary>
        /// Регистрация зависимости
        /// </summary>
        /// <param name="factoryMethod">Фабричный метод, который создаёт экземпляр зависимости</param>
        /// <typeparam name="T">Тип для которого будет внедрена зависимость</typeparam>
        public void Register<T>(Func<object> factoryMethod)
        {
            _container[typeof(T)] = factoryMethod;
        }
        
        /// <summary>
        /// Регистрация зависимости
        /// </summary>
        /// <param name="instance">Экземпляр, который будет зарегистрирован для этой зависимости</param>
        /// <typeparam name="T">Тип для которого будет внедрена зависимость</typeparam>
        public void Register<T>(T instance)
        {
            _container[typeof(T)] = () => instance;
        }
        
        /// <summary>
        /// Автоматическая регистрация зависимости
        /// </summary>
        /// <param name="obj">Объект для которого будет зарегистрированы все зависимости</param>
        public static void Register(GameObject obj)
        {
            var components = obj.GetComponents<IAutoRegistration>();
            foreach (var component in components)
            {
                component.Register();
            }
        }

        /// <summary>
        /// Внедрить зависимости
        /// </summary>
        /// <param name="obj">Объект в который будет внедрена зависимость</param>
        public void Inject(object obj)
        {
            var type = obj.GetType();
            
            InjectFields(obj, type);
            InjectMethods(obj, type);
        }
        
        /// <summary>
        /// Внедрить зависимости в игровой объект
        /// </summary>
        /// <param name="obj">Игровой объект в который будет внедрена зависимость</param>
        public void Inject(GameObject obj)
        {
            var components = obj.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component)
                {
                    Inject(component);
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InjectFields(object obj, Type type)
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var injectAttribute = field.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute == null) continue;
                var fieldType = field.FieldType;
                if (_container.TryGetValue(fieldType, out var dependency))
                {
                    field.SetValue(obj, dependency.Invoke());
                }
                else
                {
                    Debug.LogWarning(
                        $"Dependency of type {fieldType} not found in container for field {field.Name} in {type.Name}");
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InjectMethods(object obj, Type type)
        {
            var methods =
                type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var injectAttribute = method.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute == null) continue;
                var parameters = method.GetParameters();
                var parameterValues = new object[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    if (_container.TryGetValue(parameterType, out var dependency))
                    {
                        parameterValues[i] = dependency.Invoke();
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"Dependency of type {parameterType} not found in container for parameter {parameters[i].Name} in method {method.Name} of {type.Name}");
                        parameterValues[i] = default;
                    }
                }

                try
                {
                    method.Invoke(obj, parameterValues);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error invoking method {method.Name} in {type.Name}: {e.Message}");
                }
            }
        }
    }
}