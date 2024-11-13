using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class HighScore : MonoBehaviour
{
    public static HighScore Instance { get; private set; }

    private TextMeshProUGUI m_Text;

    private int m_HighScore;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            m_HighScore = PlayerPrefs.GetInt("HighScore", 0);
        }
    }

    private void OnEnable()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnDisable()
    {
        if (Instance == this)
            Instance = null;
    }

    public void TryUpdateHighScore(int score)
    {
        if (score <= m_HighScore)
            return;

        m_HighScore = score;
        PlayerPrefs.SetInt("HighScore", m_HighScore);
        UpdateText();
    }

    private void UpdateText()
    {
        if (m_Text)
            m_Text.text = $"High Score: {m_HighScore:#,0}";
    }
}
