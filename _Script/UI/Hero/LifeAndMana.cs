using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeAndMana : MonoBehaviour 
{

    [System.NonSerialized]
    public Slider life;
    [System.NonSerialized]
    public Slider mana;
    [System.NonSerialized]
    public Text lifeText;
    [System.NonSerialized]
    public Text manaText;
    [System.NonSerialized]
    public Text lifeRecoveryText;
    [System.NonSerialized]
    public Text manaRecoveryText;

    [System.NonSerialized]
    public GameObject hero;
    [System.NonSerialized]
    public HeroProperty property;

	// Use this for initialization
	void Start () 
    {
        Get();
        Text[] _text = GetComponentsInChildren<Text>();
        lifeRecoveryText = _text[2];
        manaRecoveryText = _text[3];
	}
	
	// Update is called once per frame
	void Update () 
    {
        Display();
        lifeRecoveryText.text = "+" + property.lifeRecoveryRate.ToString();
        manaRecoveryText.text = "+" + property.manaRecoveryRate.ToString();
	}

    public void Get()
    {
        hero = GameObject.Find(PlayerPrefs.GetString("player name"));
        property = hero.GetComponent<HeroProperty>();

        Slider[] _sliders = GetComponentsInChildren<Slider>();
        life = _sliders[0];
        mana = _sliders[1];

        Text[] _text = GetComponentsInChildren<Text>();
        lifeText = _text[0];
        manaText = _text[1];
    }

    public void Display()
    {
        life.value = ((float)property.curLife) / ((float)property.maxLife);
        mana.value = ((float)property.curMana) / ((float)property.maxMana);
        lifeText.text = property.curLife + "/" + property.maxLife;
        manaText.text = property.curMana + "/" + property.maxMana;
    }
}
