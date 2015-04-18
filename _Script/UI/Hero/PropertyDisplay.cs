using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PropertyDisplay : MonoBehaviour
{

    private Text[] m_texts = new Text[4];
    private Slider m_slider;

    [System.NonSerialized]
    public GameObject hero;
    [System.NonSerialized]
    public HeroProperty property;

    // Use this for initialization
    void Start ( )
    {
        hero = GameObject.Find(PlayerPrefs.GetString("player name"));
        property = hero.GetComponent<HeroProperty>();
        for (int i = 0; i < 4; i++)
        {
            m_texts[i] = GetComponentsInChildren<Text>()[i];
        }
        m_slider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update ( )
    {
        m_texts[0].text = "ATK:" + property.atk.ToString();
        m_texts[1].text = "DEF:" + property.def.ToString();
        m_texts[2].text = "SPD:" + property.moveSpeed.ToString();
        m_texts[3].text = "ATS:" + property.atkSpeed.ToString();

        float _curExp = (float)property.exp - (property.level - 1) * (100 + 100 + 10 * (property.level - 2)) / 2;
        float _curNeed = 100 + (property.level - 1) * 10;
        m_slider.value = _curExp / _curNeed;
    }
}
