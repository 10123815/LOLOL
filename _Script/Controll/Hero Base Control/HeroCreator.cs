using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroCreator : MonoBehaviour
{

    public Vector3 blueBirthPosition;
    public Vector3 redBirthPosition;

    // all heros' prefab
    public List<GameObject> heroPrefabs = new List<GameObject>();

    // 10 heros in game
    // the server should hold all these heros
    //private List<GameObject> m_heros;

    // Use this for initialization
    void Awake ( )
    {
        //m_heros = new List<GameObject>(10);

        string _playerName = PlayerPrefs.GetString("player name");
        int _heroIndex = PlayerPrefs.GetInt("hero index");
        string _group = PlayerPrefs.GetString("group");

        Vector3 _birthPosition = new Vector3();
        if (_group.Equals("red"))
            _birthPosition = redBirthPosition;
        else if (_group.Equals("blue"))
            _birthPosition = blueBirthPosition;

        GameObject _hero = (GameObject)Object.Instantiate(heroPrefabs[_heroIndex], _birthPosition, heroPrefabs[_heroIndex].transform.rotation);
        _hero.name = _playerName;
        _hero.tag = _group;

        Application.targetFrameRate = 60;
    }
}
