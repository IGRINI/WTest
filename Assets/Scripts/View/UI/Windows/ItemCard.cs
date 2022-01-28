using DG.Tweening;
using Pool;
using TMPro;
using UnityEngine;

namespace View.UI.Windows
{
    public class ItemCard : AnimatedUiElement, IPoolable
    {
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemSlotType;
        [SerializeField] private TMP_Text _itemRarity;
        [SerializeField] private TMP_Text _itemLevel;

        protected override float ShowTweenDuration => 0.8f;

        public void OnSpawn()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(string name, string slotType, string rarity, int level, TweenCallback showCallback = null)
        {
            _itemName.text = name;
            _itemSlotType.text = slotType;
            _itemRarity.text = rarity;
            _itemLevel.text = level.ToString();
            Show(showCallback);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
        }
    }
}