using System;
using MadFireOn;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AbsController : MonoBehaviour, IUnityAdsListener
{
    string gameId = "3703542";
    public const string RewardedPlacementId = "rewardedVideo";
    public const string RetryPlacementId = "retryVideo";
    bool testMode = true;

    public Action<ShowResult> OnUnityAdsFinish;
    public static AbsController Instance;

    // Initialize the Ads listener and service:
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowRewardedAd(string myPlacementId) 
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        OnUnityAdsFinish?.Invoke(showResult);
        ClickPoint.Instance.isCreatAward = false;
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            if (placementId == RewardedPlacementId)
            {
                AddPlayerScore();
            }
            else if (placementId == RetryPlacementId)
            {
                RetryGame();
;            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        {
            Debug.LogError("OnUnityAdsReady");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    private void RetryGame() 
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);

        PlayerCanSeeAds.Instance.IsCanWatchMove = false;
    }

    private void AddPlayerScore()
    {
        int AddplayerScore = UnityEngine.Random.Range(45, 50) * 5;

        GameManager.instance.AddScore(AddplayerScore);
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
