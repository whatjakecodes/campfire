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

        private void Start()
        {
            _camera.transform.LookAt(m_FocusTarget);
        }
    }
}