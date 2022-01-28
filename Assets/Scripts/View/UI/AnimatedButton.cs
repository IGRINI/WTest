using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    [RequireComponent(typeof(Button))]
    public class AnimatedButton : AnimatedUiElement
    {
        [HideInInspector]
        public Button Button;

        protected override void Awake()
        {
            base.Awake();
            Button = GetComponent<Button>();
        }
    }
}