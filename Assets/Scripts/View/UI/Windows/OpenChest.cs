using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View.UI.Windows
{
    public class OpenChest : AnimatedUiElement
    {
        [SerializeField] private Image _chestImage;
        public Image ChestImage => _chestImage;
        
        [SerializeField] private Button _openChestButton;

        public void SetChestImage(Image image)
        {
            _chestImage = image;
        }

        public void SetButtonInteractable(bool interactable)
        {
            _openChestButton.interactable = interactable;
        }

        public void AddClickListener(UnityAction callback)
        {
            _openChestButton.onClick.AddListener(callback);
        }

        public void RemoveClickListener(UnityAction callback)
        {
            _openChestButton.onClick.RemoveListener(callback);
        }
    }
}