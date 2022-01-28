using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    [RequireComponent(typeof(Button))]
    public class AnimatedShowButton : AnimatedShowUi
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