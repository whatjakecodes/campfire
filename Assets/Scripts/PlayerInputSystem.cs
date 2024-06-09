using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    [SerializeField] private LayerMask m_WoodCubesLayerMask;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePos);
            var hits = Physics.Raycast(ray, out var hitInfo, 100f, m_WoodCubesLayerMask);

            if (hits)
            {
                var hitCube = hitInfo.collider.GetComponent<WoodCube>();
                hitCube.QueuePhysicsDisintegration();
            }
        }
    }

}