using System;
using Services.DependencyInjection;
using Services.Interfaces;
using TMPro;
using UnityEngine;

namespace View
{
    public class HpMpView : MonoBehaviour
    {
        [Inject] private IPlayerService _player;
        private TMP_Text _text;
        private void Start()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _text.text = $"Здоровье: {Mathf.Round(_player.Health)} Мана: {Mathf.Ceil(_player.Mana)}";
        }
    }
}