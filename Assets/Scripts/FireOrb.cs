using UnityEngine;

public class FireOrb : MonoBehaviour
{
    [Range(0.1f, 2.0f)] public float m_TransitionTime = 1.0f;

    public Color m_Color1 = Color.red;
    public Color m_Color2 = Color.yellow;
    public MeshRenderer m_Renderer;

    private float elapsedTime;
    private Color previous;
    private Color target;
    private bool direction;

    private void Start()
    {
        previous = m_Color1;
        target = m_Color2;
    }

    private void Update()
    {
        if (elapsedTime < m_TransitionTime)
        {
            var nextColor = Color.Lerp(previous, target, elapsedTime / m_TransitionTime);
            m_Renderer.material.color = nextColor;
            
            elapsedTime += Time.deltaTime;
        }
        else
        {
            direction = !direction;
            target = direction ? m_Color1 : m_Color2;
            previous = direction ? m_Color2 : m_Color1;
            elapsedTime = 0;
        }
    }
}