using UnityEngine;

namespace Utilities
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraViewportHandler : MonoBehaviour
    {
        public enum Constraint { Landscape, Portrait }

        [SerializeField] private Color _wireColor = Color.white;
        [SerializeField] private float _unitsSize = 1; // size of your scene in unity units
        [SerializeField] private Constraint _constraint = Constraint.Portrait;
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
            if (_constraint == Constraint.Landscape)
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
            Gizmos.color = _wireColor;
            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            if (_camera.orthographic)
            {
                float spread = _camera.farClipPlane - _camera.nearClipPlane;
                float center = (_camera.farClipPlane + _camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(_camera.orthographicSize * 2 * _camera.aspect, _camera.orthographicSize * 2, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, _camera.fieldOfView, _camera.farClipPlane, _camera.nearClipPlane, _camera.aspect);
            }

            Gizmos.matrix = temp;
        }
        #endregion

    }
}