

using Services.EventBus;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInit : MonoBehaviour
{
    private void Awake()
    {
        var services = ServiceLocator.Current;

        var eventBus = new EventBus();
        eventBus.Subscribe(EventList.PlayerDead, () =>
        {
            ServiceLocator.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        services.Register<IEventBus>(eventBus);
    }
}