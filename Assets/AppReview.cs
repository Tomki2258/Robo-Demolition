using System;
using System.Collections;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using Google.Play.Review;
#endif

public class AppReview : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;


    private void Start()
    {
        //StartCoroutine(RequestReview());
    }

    IEnumerator RequestReview()
    {
        _reviewManager = new ReviewManager(); 
        
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();
        
        
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
// The flow has finished. The API does not indicate whether the user
// reviewed or not, or even whether the review dialog was shown. Thus, no
// matter the result, we continue our app flow.
    }
}