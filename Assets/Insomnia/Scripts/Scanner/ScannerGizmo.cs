using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerGizmo : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private SpriteRenderer m_baseGizmo = null;
    [SerializeField] private SpriteRenderer[] m_rotGizmos = null;

    [Header("Settings")]
    [SerializeField] private Color m_gizmoColor = Color.green;
    [SerializeField, Range(0f, 1f)] private float m_baseAlphaThreshold = 0.2f;
    [SerializeField, Range(0f, 1f)] private float m_movAlphaThreshold = 0.1f;
    [SerializeField, Range(1f, 50f)] private float m_maxRotSpeed = 1f;
    [SerializeField, Range(1f, 50f)] private float m_minRotSpeed = 1f;

    [Header("Stat")]
    [SerializeField] private bool m_rotate = false;
    private Vector2 m_basePosition = Vector2.zero;
    private float m_baseNormalDistance = 0f;
    [SerializeField] private short[] m_rotDirection;

    public bool Rotate { 
        get => m_rotate;  
        set {
            if(m_rotate == value)
                return;

            m_rotate = value;
            if(m_rotate)
                StartCoroutine(CoStartRotate());
            else
                StopCoroutine(CoStartRotate());
        }
    }

    #region Unity Event Functions
    private void Awake() {
        m_basePosition = m_baseGizmo.transform.position;
        m_baseNormalDistance = m_baseGizmo.transform.localScale.x / 2;
        m_rotDirection = new short[m_rotGizmos.Length];

        if(m_minRotSpeed > m_maxRotSpeed) {
            float temp = m_minRotSpeed;
            m_minRotSpeed = m_maxRotSpeed;
            m_maxRotSpeed = temp;
        }
    }

    private void Start() {
        for(int i = 0; i < m_rotGizmos.Length; i++) {
            m_rotDirection[i] = (short)(m_rotGizmos[i].transform.position.x - m_basePosition.x < 0 ? 1 : -1);
        }
    }

    private void OnValidate() {
        Color result = new Color(m_gizmoColor.r, m_gizmoColor.g, m_gizmoColor.b, m_baseAlphaThreshold);
        m_baseGizmo.color = result;

        result.a = m_movAlphaThreshold;
        for(int i = 0; i < m_rotGizmos.Length; i++) {
            m_rotGizmos[i].color = result;
        }

        m_rotDirection = new short[m_rotGizmos.Length];
    }

    #endregion

    public void ActivateScan() {
        m_baseGizmo.gameObject.SetActive(true);

        for(int i = 0; i < m_rotGizmos.Length; i++) {
            m_rotGizmos[i].gameObject.SetActive(true);
        }
        RotateStart();
    }

    public void DeactivateScan() {
        RotateEnd();
        m_baseGizmo.gameObject.SetActive(false);

        for(int i = 0; i < m_rotGizmos.Length; i++) {
            m_rotGizmos[i].gameObject.SetActive(false);
        }
    }

    public void RotateStart() { Rotate = true; }
    public void RotateEnd() { Rotate = false; }

    private IEnumerator CoStartRotate() {
        while(m_rotate) {
            for(int i = 0; i < m_rotGizmos.Length; i++) {
                if(Mathf.Abs(m_rotGizmos[i].transform.position.x - m_basePosition.x) >= m_baseNormalDistance)
                    m_rotDirection[i] *= -1;

                float speed  = Mathf.Clamp((1 - (Mathf.Abs(m_rotGizmos[i].transform.position.x - m_basePosition.x) / m_baseNormalDistance)) * m_maxRotSpeed, m_minRotSpeed, m_maxRotSpeed);

                Vector3 nextPos = m_rotGizmos[i].transform.position + new Vector3(m_rotDirection[i] * speed * Time.deltaTime, 0f);
                nextPos.x = Mathf.Clamp(nextPos.x, m_basePosition.x - m_baseNormalDistance, m_basePosition.x + m_baseNormalDistance);
                m_rotGizmos[i].transform.position = nextPos;
                yield return null;
            }
        }

        yield break;
    }
}
