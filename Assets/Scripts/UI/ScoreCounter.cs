using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }

    private TextMeshProUGUI m_Text;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateScore(0);
    }

    private void OnDisable()
    {
        if (Instance == this)
            Instance = null;
    }

    public void UpdateScore(int value)
    {
        if (m_Text)
            m_Text.text = value.ToString("#,0");

        HighScore.Instance.TryUpdateHighScore(value);
    }
}
