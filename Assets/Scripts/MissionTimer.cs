using TMPro;
using UnityEngine;

public class MissionTimer : MonoBehaviour
{
    bool isTiming = false;
    float missionTime = 0f;

    TMP_Text timerText;

    void Start()
    {
        timerText = GetComponentInChildren<TMP_Text>();

        timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isTiming)
        {
            return;
        }

        missionTime -= Time.deltaTime;

        // Mission failed!
        if (missionTime <= 0f)
        {
            missionTime = 0f;
            isTiming = false;

            timerText.text = "OUT OF TIME!!!";
        }

        timerText.text = "00:" + missionTime;
    }

    public void SetMissionTimeAndStartCountdown(float newMissionTime)
    {
        missionTime = newMissionTime;
        isTiming = true;

        timerText.gameObject.SetActive(true);
    }

    // Mission success!
    public void StopTimer()
    {
        isTiming = false;

        timerText.gameObject.SetActive(false);
    }
}