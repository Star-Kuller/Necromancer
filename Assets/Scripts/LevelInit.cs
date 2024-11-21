using System;
using Services.DependencyInjection;
using Services.EventBus;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInit : MonoBehaviour, IAutoRegistration
{
    public void Awake()
    {
        DependencyInjector.Current.AutoRegisterAndInject(SceneManager.GetActiveScene());
    }

    public void Register()
    {
        var eventBus = new EventBus();
        eventBus.Register<IEventBus>();
    }
    
    [Inject]
    private void Initialization(IEventBus eventBus)
    {
        eventBus.Subscribe(GameEvent.PlayerDead, _ =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}