using System;
using Services.Interfaces;
using Services.ServiceLocator;
using TMPro;
using UnityEngine;

namespace View
{
    public class HpMpView : MonoBehaviour
    {
        private IPlayerService _player;
        private TMP_Text _text;
        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            var services = ServiceLocator.Current;
            _player = services.Get<IPlayerService>();
        }

        private void Update()
        {
            _text.text = $"Здоровье: {Mathf.Round(_player.Health)} Мана: {Mathf.Ceil(_player.Mana)}";
        }
    }
}