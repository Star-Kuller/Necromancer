using System;
using Services.EventBus;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;

public class LevelInit : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Reset();
        var services = ServiceLocator.Current;
        
        services.Register<IEventBus>(new EventBus());
    }
}