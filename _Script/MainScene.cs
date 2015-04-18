using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainScene : MonoBehaviour
{

    public void SelectHero()
    {
        int _index = int.Parse(gameObject.name);
        print(_index);
        PlayerPrefs.SetInt("hero index", _index);
    }

    public void SetPlayerName()
    {
        string _playerName = GetComponent<InputField>().text;
        PlayerPrefs.SetString("player name", _playerName);
    }


    public void StartGame()
    {
        Toggle _t = GameObject.Find("red").GetComponent<Toggle>();
        string _goutp = (_t.isOn) ? "red" : "blue";
        PlayerPrefs.SetString("group", _goutp);

        // switch loading scene, then switch to game scene
        Application.LoadLevel("loading");
    }
}
