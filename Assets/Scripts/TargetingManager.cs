using System.Collections;
using UnityEngine;

public class TargetingManager : MonoBehaviour
{
    public GameObject targetIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetIndicator.transform.Rotate(0,0,1);
    }

    IEnumerator SmoothMoveIndicator(Transform allyTransform)
    {
        float duration = 0.1f;
        Vector3 start = targetIndicator.transform.position;
        Vector3 end = allyTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            targetIndicator.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetIndicator.transform.SetParent(allyTransform);
        targetIndicator.transform.localPosition = Vector3.zero;
    }

    // TODO: Find a way to merge these functions, may require all entities to do inheritance class
    public void MoveIndicatorToAlly(Ally ally)
    {
        if (targetIndicator != null && ally != null)
        {
            StartCoroutine(SmoothMoveIndicator(ally.transform));
        }
    }

    public void MoveIndicatorToBoss(Boss boss)
    {
        if (targetIndicator != null && boss != null)
        {
            StartCoroutine(SmoothMoveIndicator(boss.transform));
        }
    }
}
