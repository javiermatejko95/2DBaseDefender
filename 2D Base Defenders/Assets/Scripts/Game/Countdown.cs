using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Countdown : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI countdownTxt = null;
    [SerializeField] private Image fillImage = null;

    [Space, Header("Configuration")]
    [SerializeField] private int startingTime = 3;
    #endregion

    #region PRIVATE_FIELDS
    private float currentTime = 0f;

    protected IEnumerator countdownCoroutine = null;
    #endregion

    #region ACTIONS
    private Action onFinish = null;
    #endregion

    #region INIT
    public void Initialize(Action onFinish)
    {
        this.onFinish = onFinish;
    }
    #endregion

    #region PUBLIC_METHODS
    public void StartCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = CountdownCoroutine(startingTime);

        StartCoroutine(countdownCoroutine);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region PRIVATE_METHODS
    private IEnumerator CountdownCoroutine(float _time = 0)
    {        
        currentTime = _time;
        fillImage.fillAmount = 1;

        while (currentTime > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Math.Ceiling(currentTime));
            UpdateTimeDisplay(timeSpan);
            currentTime -= Time.deltaTime;
            UpdateFiller(currentTime);
            yield return null;
        }

        currentTime = 0;
        countdownTxt.text = currentTime.ToString();
        onFinish?.Invoke();
    }

    private void UpdateTimeDisplay(TimeSpan timeSpan)
    {
        countdownTxt.text = timeSpan.TotalSeconds.ToString();        
    }

    private void UpdateFiller(float time)
    {
        fillImage.fillAmount = time / startingTime;
    }
    #endregion
}
