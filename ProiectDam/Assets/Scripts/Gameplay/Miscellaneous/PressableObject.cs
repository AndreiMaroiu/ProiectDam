using UnityEngine;

namespace Gameplay
{
    public abstract class PressableObject : MonoBehaviour
    {
        public abstract void OnClick();

        private void OnMouseUp()
        {
            if (Time.deltaTime == 0.0f)
            {
                return;
            }

            OnClick();
        }
    }
}
