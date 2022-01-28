using System;
using Data;
using Pool;
using UnityEngine;
using UnityEngine.Networking;
using View.UI.Windows;

namespace Services
{
    public sealed class ChestService : MonoBehaviour
    {
        public const string ChestItemsUrl = "http://dev-mob.bfa.games/hamsters//chests/lut_test";
        
        [SerializeField] private GameObject _itemCardPrefab;
        
        [SerializeField] private OpenChest _openChestWindow;
        
        [SerializeField] private ChestItems _chestItemsWindow;

        private PrefabPool<ItemCard> _itemCardPool;
        private ItemCard[] _activeItemCards = Array.Empty<ItemCard>();
            
        private void Start()
        {
            print("Initialized item cards pool");
            _itemCardPool = new PrefabPool<ItemCard>(_itemCardPrefab, 5, _chestItemsWindow.ItemCardsHolder);
            print("Add listener to open button");
            _openChestWindow.AddClickListener(OpenChest);
            print("Add listener to close items window button");
            _chestItemsWindow.SetCloseListener(() => _openChestWindow.Show());
        }

        private void OpenChest()
        {
            print("Set button interactable is false and play animation");
            _openChestWindow.SetButtonInteractable(false);
            _openChestWindow.Hide(() => _openChestWindow.SetButtonInteractable(true));
            print("Send opened items request");
            UnityWebRequest.Get(ChestItemsUrl)
                .SendWebRequest()
                .completed += OnOpenRequestCompleted;
        }

        private void OnOpenRequestCompleted(AsyncOperation requestOperation)
        {
            UnityWebRequestAsyncOperation request = (UnityWebRequestAsyncOperation)requestOperation;
            if (request.webRequest.responseCode != 200)
                throw new Exception("Response code is not 200");
            print("Request completed");
            print("Get request text and deserialize it from json");
            var chestInfo = JsonUtility.FromJson<ChestInfo>(request.webRequest.downloadHandler.text);
            print("Despawn previous cards");
            foreach (ItemCard activeItemCard in _activeItemCards)
            {
                _itemCardPool.Despawn(activeItemCard);
            }
            print("Spawn new cards and start recursive animation");
            _activeItemCards = new ItemCard[chestInfo.chest_items.Length];
            for (var i = 0; i < chestInfo.chest_items.Length; i++)
            {
                _activeItemCards[i] = _itemCardPool.Spawn();
            }
            _chestItemsWindow.Show(() => ItemsShowRecursive(chestInfo));
            print("Block interactable");
            _chestItemsWindow.SetInteractable(false);
        }

        private void ItemsShowRecursive(ChestInfo chestInfo, int index = 0)
        {
            if (index >= _activeItemCards.Length)
            {
                print("Show close button when animation ends");
                _chestItemsWindow.ShowCloseButton();
                print("Unblock interactable");
                _chestItemsWindow.SetInteractable(true);
                return;
            }
            var itemInfo = chestInfo.chest_items[index];
            _activeItemCards[index].Initialize(itemInfo.itemkey, itemInfo.slottype, itemInfo.rarity, itemInfo.level, () => ItemsShowRecursive(chestInfo, ++index));
            _chestItemsWindow.SmoothScrollToRight();
        }
    }
}
