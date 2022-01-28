using System;
using DG.Tweening;
using UnityEngine;

namespace View.UI
{
    public class AnimatedLoading : MonoBehaviour
    {
        private Tween _tween;

        private readonly Vector3 _defaultScale = new Vector3(1, 1, 1);
        private readonly Vector3 _minScale = new Vector3(0, 0, 0);
        private const float _scaleDuration = 0.3f;
        
        private void OnEnable()
        {
            transform.localScale = _defaultScale;
            StartScaleTweenWithType(ScaleType.Decrease);
        }

        private void StartScaleTweenWithType(ScaleType scaleType)
        {
            _tween?.Kill();
            switch (scaleType)
            {
                case ScaleType.Decrease:
                    StartScaleTweenWithContinue(_minScale, ScaleType.Increase);
                    break;
                case ScaleType.Increase:
                    StartScaleTweenWithContinue(_defaultScale, ScaleType.Decrease);
                    break;
            }
        }

        private void StartScaleTweenWithContinue(Vector3 endValue, ScaleType nextType)
        {
            _tween = transform.DOScale(endValue, _scaleDuration);
            _tween.OnComplete(() => StartScaleTweenWithType(nextType));
        }

        private void OnDisable()
        {
            _tween.Kill();
        }
        
        private enum ScaleType
        {
            Increase,
            Decrease
        }
    }
}