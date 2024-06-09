using System;
using UnityEngine;

namespace CampfireCamera
{
    public class CameraRoot : MonoBehaviour
    {
        [SerializeField] private Transform m_FocusTarget;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
        }
    }
}