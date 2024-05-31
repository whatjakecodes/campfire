using UnityEngine;

namespace CampfireCamera
{
    public class CameraRotator : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _smoothing = 5f;

        private float _targetAngle;
        private float _currentAngle;

        private void Awake()
        {
            _targetAngle = transform.eulerAngles.y;
            _currentAngle = _targetAngle;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                _targetAngle += 45;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                _targetAngle -= 45;
            }
            
            if (Input.GetMouseButton(2))
            {
                var mouseX = Input.GetAxis("Mouse X");
                _targetAngle += mouseX * _speed;
            }
            else
            {
                _targetAngle = Mathf.Round(_targetAngle / 45);
                _targetAngle *= 45;
            }

            if (_targetAngle < 0)
            {
                _targetAngle += 360;
            }

            if (_targetAngle > 360)
            {
                _targetAngle -= 360;
            }

            _currentAngle = Mathf.LerpAngle(_currentAngle, _targetAngle, Time.deltaTime * _smoothing);
            transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.up);
        }

        public void SetTargetAngle(Quaternion rotation)
        {
            _targetAngle = rotation.eulerAngles.y;
            _currentAngle = _targetAngle;
            transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.up);
        }
    }
}