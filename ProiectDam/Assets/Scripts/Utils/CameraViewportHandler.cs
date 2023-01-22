using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(Camera))]
    public class CameraViewportHandler : MonoBehaviour
    {
        public enum Constraint { Width, Height }

        [SerializeField] private Color _wireColor = Color.white;
        [Tooltip("size of your scene in unity units")]
        [SerializeField] private float _unitsSize = 1;
        [SerializeField] private Constraint _constraint = Constraint.Width;
        [SerializeField] private bool _executeInUpdate;

        private Camera _camera;

        #region METHODS
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            ComputeResolution();
        }

        private void ComputeResolution()
        {
            if (_constraint is Constraint.Width)
            {
                _camera.orthographicSize = 1f / _camera.aspect * _unitsSize / 2f;
            }
            else
            {
                _camera.orthographicSize = _unitsSize / 2f;
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (_executeInUpdate)
            {
                ComputeResolution();
            }
        }
#endif

        void OnDrawGizmos()
        {
            var camera = GetComponent<Camera>();

            Gizmos.color = _wireColor;
            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            if (camera.orthographic)
            {
                float spread = camera.farClipPlane - camera.nearClipPlane;
                float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
            }

            Gizmos.matrix = temp;
        }
        #endregion

    }
}