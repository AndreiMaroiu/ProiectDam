using UnityEngine;

namespace Gameplay
{
    public abstract class PressableObject : MonoBehaviour
    {
        public abstract void OnClick();

        private void OnMouseUp()
        {
            OnClick();
        }
    }
}
