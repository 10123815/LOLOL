using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Load : MonoBehaviour
{

    private AsyncOperation m_async;

    private Slider m_progress;
    private Text m_proText;

    // Use this for initialization
    void Start ( )
    {
        m_progress = GetComponent<Slider>();
        m_proText = GetComponentInChildren<Text>();
        StartCoroutine(LoadGame());
    }

    // Update is called once per frame
    void Update ( )
    {
        m_progress.value = m_async.progress;
        m_proText.text = (int)(m_async.progress * 100) + "%";
    }

    private IEnumerator LoadGame()
    {
        m_async = Application.LoadLevelAsync("game");
        yield return m_async;
    }
}
