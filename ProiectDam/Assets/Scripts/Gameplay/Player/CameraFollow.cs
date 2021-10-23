using UnityEngine;

namespace Gameplay.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;

        private void LateUpdate()
        {
            Vector3 target = _playerController.transform.position;
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }
    }
}
