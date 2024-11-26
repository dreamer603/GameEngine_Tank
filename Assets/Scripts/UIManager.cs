using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("UI")]
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public Image hpSliderFill;
    public Slider chargeSlider;
    public TextMeshProUGUI scoreText;
    
    private Player _player;
    private bool _isWarning;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _player = FindObjectOfType<Player>();
        _isWarning = false;
        hpSlider.value = 1;
        chargeSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = _player.hp / _player.maxHp;
        
        if (_player.hp <= 30 && !_isWarning)
        {
            _isWarning = true;
            hpSliderFill.color = Color.red;
        }
        hpText.text = _player.hp + " / " + _player.maxHp;

        scoreText.text = "Score : " + GameManager.Instance.Score;
    }

    public IEnumerator ChargePowerGaugeUI()
    {
        float duration = 1f;
        float currentTime = 0.0f;

        chargeSlider.value = 0f;
        
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            chargeSlider.value = currentTime / duration;
            yield return null;
        }
    }
}
