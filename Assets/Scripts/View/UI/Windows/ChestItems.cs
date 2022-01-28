using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace View.UI.Windows
{
    public class ChestItems : AnimatedShowUi
    {
        [SerializeField] private Transform _itemCardsHolder;
        public Transform ItemCardsHolder => _itemCardsHolder;
        
        [FormerlySerializedAs("_closeButton")] [SerializeField] private AnimatedShowButton closeShowButton;
        private TweenCallback _closeCallback;
        
        [SerializeField] private ScrollRect _scrollRect;
        private const float ScrollEndValue = 1;
        private const float ScrollTweenDuration = 0.1f;
        private Tween _scrollTween;

        protected override void Awake()
        {
            base.Awake();
            closeShowButton.Button.onClick.AddListener(() => Hide(_closeCallback));
        }
        
        public void SetCloseListener(TweenCallback callback = null)
        {
            _closeCallback = callback;
        }

        public override void Show(TweenCallback callback = null)
        {
            base.Show(callback);
            closeShowButton.gameObject.SetActive(false);
            closeShowButton.Button.interactable = false;
        }

        public void ShowCloseButton()
        {
            closeShowButton.Show(() => closeShowButton.Button.interactable = true);
        }

        public void SmoothScrollToRight()
        {
            _scrollTween?.Kill();
            _scrollTween = DOTween.To(() => _scrollRect.horizontalNormalizedPosition,
                x => _scrollRect.horizontalNormalizedPosition = x, ScrollEndValue, ScrollTweenDuration);
        }

        public void SetInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
            _canvasGroup.blocksRaycasts = interactable;
        }
    }
}