using UnityEngine;

namespace Gameplay
{
    public abstract class PressebleObject : MonoBehaviour
    {
        protected abstract void OnClick();

        private void OnMouseUp() => OnClick();
    }
}
