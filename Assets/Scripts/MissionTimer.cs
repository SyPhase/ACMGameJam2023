using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionTimer : MonoBehaviour
{
    bool isTiming = false;
    float missionTime = 0f;

    TMP_Text timerText;
    BigBlock bigBlock;
    SmallBlock smallBlock;

    void Start()
    {
        timerText = GetComponentInChildren<TMP_Text>();

        timerText.gameObject.SetActive(false);

        bigBlock = FindObjectOfType<BigBlock>(true);
        smallBlock = FindObjectOfType<SmallBlock>(true);
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
            StartCoroutine(RestartGame(5f));
        }
        else
        {
            string time = missionTime.ToString();
            int index = -1;

            if (time.Contains('.'))
            {
                index = time.IndexOf('.');
            }
            if (index > 0)
            {
                time = time.Substring(0, index);
            }

            timerText.text = "00:" + time;
        }
    }

    IEnumerator RestartGame(float seconds)
    {
        missionTime = 0f;
        isTiming = false;

        timerText.text = "OUT OF TIME!!!";

        smallBlock.DisableBlock();
        bigBlock.DisableBlock();

        // Wait 5 seconds
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetMissionTimeAndStartCountdown(float newMissionTime)
    {
        missionTime = newMissionTime;
        isTiming = true;

        timerText.gameObject.SetActive(true);
    }

    // Mission success!
    public bool StopTimer()
    {
        if (isTiming && timerText.gameObject.activeSelf)
        {
            isTiming = false;
            timerText.gameObject.SetActive(false);

            return true;
        }
        else
        {
            return false;
        }
    }
}