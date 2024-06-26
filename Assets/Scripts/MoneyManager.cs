using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyManager : MonoBehaviour
{
    private float _coins;
    [SerializeField] private float _coinsMultiplier;
    [SerializeField] private float _coinsAutoMultiplier;
    [SerializeField] private float _AutoWait;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private GameObject _clickFX;
    [SerializeField] private RectTransform _buttonPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("coins"))
        {
            //PlayerPrefs.DeleteAll();
            CountOfflineCoins();
            _coins = PlayerPrefs.GetFloat("coins");
            _coinsText.text = _coins.ToString("N8");
        }

        StartCoroutine(AutoCoins());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EarnMoney()
    {
        Instantiate(_clickFX, _buttonPosition.position.normalized, Quaternion.identity);
        _coins += _coinsMultiplier;
        PlayerPrefs.SetFloat("coins", _coins);
        _coinsText.text = _coins.ToString("N8");
    }

    private IEnumerator AutoCoins()
    {
        Instantiate(_clickFX, _buttonPosition.position.normalized, Quaternion.identity);
        _coins += _coinsAutoMultiplier;
        PlayerPrefs.SetFloat("coins", _coins);
        _coinsText.text = _coins.ToString("N8");
        yield return new WaitForSeconds(_AutoWait);

        StartCoroutine(AutoCoins());
    }

    private void CountOfflineCoins()
    {
        TimeSpan timespan;
        if (PlayerPrefs.HasKey("LastSessionData"))
        {
            timespan = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSessionData"));
            _coins = _coinsAutoMultiplier * (int)timespan.TotalSeconds;
            PlayerPrefs.SetFloat("coins", _coins);
            _coinsText.text = _coins.ToString("N8");
            Debug.Log(timespan.TotalSeconds);
        }
        else
        {
            Debug.Log("LastSession Key still not exist");
        }
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetString("LastSessionDate", DateTime.Now.ToString());
        }
    }
#else
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastSessionDate", DateTime.Now.ToString());

    }
#endif

}
