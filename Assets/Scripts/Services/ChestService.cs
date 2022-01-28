using System;
using System.Collections;
using System.Collections.Generic;
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
        public const string ItemsImagesUrl = "https://raw.githubusercontent.com/IGRINI/WTest/master/Assets/Sprites/Items/";
        
        [SerializeField] private GameObject _itemCardPrefab;
        
        [SerializeField] private OpenChest _openChestWindow;
        
        [SerializeField] private ChestItems _chestItemsWindow;
        
        [SerializeField] private GameObject _loadingGameObject;

        private PrefabPool<ItemCard> _itemCardPool;
        private ItemCard[] _activeItemCards = Array.Empty<ItemCard>();

        private Dictionary<string, Sprite> _itemSprites = new Dictionary<string, Sprite>();

        private void Start()
        {
            print("Initialized item cards pool");
            _itemCardPool = new PrefabPool<ItemCard>(_itemCardPrefab, 5, _chestItemsWindow.ItemCardsHolder);
            
            print("Add listener to open button");
            _openChestWindow.AddClickListener(OpenChest);
            
            print("Set listener to close items window button");
            _chestItemsWindow.SetCloseListener(() => _openChestWindow.Show());
        }

        private void OpenChest()
        {
            print("Set button interactable is false and play animation");
            _openChestWindow.SetButtonInteractable(false);
            _openChestWindow.Hide(() => _openChestWindow.SetButtonInteractable(true));
            
            print("Send opened items request with coroutine");
            StartCoroutine(OpenRequest());
        }

        private IEnumerator OpenRequest()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(ChestItemsUrl))
            {
                yield return request.SendWebRequest();
                
                print("Request completed");
            
                if (request.responseCode != 200)
                    throw new Exception("Opened items page not found");

                print("Get request text and deserialize it from json");
                var chestInfo = JsonUtility.FromJson<ChestInfo>(request.downloadHandler.text);

                print("Download items images and store it in memory");
                _loadingGameObject.SetActive(true);
                
                yield return DownloadItemImagesIfNotExists(chestInfo);

                _loadingGameObject.SetActive(false);
            
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
            
                print("Block interactable until open end");
                _chestItemsWindow.SetInteractable(false);
            }
        }

        private IEnumerator DownloadItemImagesIfNotExists(ChestInfo chestInfo)
        {
            foreach (ChestInfo.ChestItemInfo item in chestInfo.chest_items)
            {
                if (_itemSprites.ContainsKey(item.itemkey))
                    continue;

                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture($"{ItemsImagesUrl}{item.itemkey}.png"))
                {
                    yield return request.SendWebRequest();
                    
                    if (request.responseCode != 200)
                        throw new Exception("Image not founded");
            
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    _itemSprites.Add(item.itemkey, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }

        private void ItemsShowRecursive(ChestInfo chestInfo, int index = 0)
        {
            if (index >= _activeItemCards.Length)
            {
                print("Show close button when animation ends");
                _chestItemsWindow.ShowCloseButton();
                
                print("Enable interactable");
                _chestItemsWindow.SetInteractable(true);
                return;
            }
            var itemInfo = chestInfo.chest_items[index];
            
            _activeItemCards[index].Initialize(itemInfo.itemkey, itemInfo.slottype, itemInfo.rarity, itemInfo.level, _itemSprites[itemInfo.itemkey], () => ItemsShowRecursive(chestInfo, ++index));
            
            _chestItemsWindow.SmoothScrollToRight();
        }
    }
}
