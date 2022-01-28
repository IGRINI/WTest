using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI.Windows
{
    public class ChestItems : AnimatedUiElement
    {
        [SerializeField] private Transform _itemCardsHolder;
        public Transform ItemCardsHolder => _itemCardsHolder;
        
        [SerializeField] private AnimatedButton _closeButton;
        private TweenCallback _closeCallback;
        
        [SerializeField] private ScrollRect _scrollRect;
        private const float ScrollEndValue = 1;
        private const float ScrollTweenDuration = 0.1f;
        private Tween _scrollTween;

        protected override void Awake()
        {
            base.Awake();
            _closeButton.Button.onClick.AddListener(() => Hide(_closeCallback));
        }
        
        public void SetCloseListener(TweenCallback callback = null)
        {
            _closeCallback = callback;
        }

        public override void Show(TweenCallback callback = null)
        {
            base.Show(callback);
            _closeButton.gameObject.SetActive(false);
            _closeButton.Button.interactable = false;
        }

        public void ShowCloseButton()
        {
            _closeButton.Show(() => _closeButton.Button.interactable = true);
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