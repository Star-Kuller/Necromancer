using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class InputController : MonoBehaviour, IInputController, IAutoRegistration
    {
        [Inject] private ICardController _cardController;
        public void Register() => this.Register<IInputController>();
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) _cardController.PlayCard(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) _cardController.PlayCard(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) _cardController.PlayCard(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) _cardController.PlayCard(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) _cardController.PlayCard(4);
        }
    }
}
