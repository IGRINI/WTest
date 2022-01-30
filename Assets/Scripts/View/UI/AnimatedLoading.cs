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
        
        private void Awake()
        {
            transform.localScale = _defaultScale;
            _tween = transform.DOScale(_minScale, _scaleDuration);
            _tween.SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            _tween.Pause();
        }

        private void OnEnable()
        {
            _tween.Restart();
        }
    }
}