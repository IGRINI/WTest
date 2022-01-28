using System;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace View.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AnimatedShowUi : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;
        private Tween _tween;

        protected virtual float ShowTweenDuration => DefaultOpacityTweenDuration;
        protected virtual float HideTweenDuration => DefaultOpacityTweenDuration;
        
        private const float DefaultOpacityTweenDuration = 0.2f;
        private const float AlphaMaxValue = 1;
        private const float AlphaMinValue = 0;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(TweenCallback callback = null)
        {
            _tween?.Kill();
            gameObject.SetActive(true);
            _canvasGroup.alpha = AlphaMinValue;
            _tween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, AlphaMaxValue, ShowTweenDuration);
            if (callback != null)
            {
                _tween.OnComplete(callback);
            }
        }
        public virtual void Hide(TweenCallback callback = null)
        {
            _tween?.Kill();
            _canvasGroup.alpha = AlphaMaxValue;
            _tween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, AlphaMinValue, HideTweenDuration);
            _tween.OnComplete(() =>
            {
                gameObject.SetActive(false);
                callback?.Invoke();
            });
        }
    }
}