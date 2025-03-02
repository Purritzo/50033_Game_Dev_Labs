using System.Collections;
using UnityEngine;

public class TargetingManager : MonoBehaviour
{
    public GameObject targetIndicator;
    public Entity currentTargetedEntity;
    private bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetIndicator.transform.Rotate(0,0,1);
        if (moving == false && currentTargetedEntity != null){
            targetIndicator.transform.position = currentTargetedEntity.transform.position;
        }
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

        targetIndicator.transform.position = end;
        moving = false;
        //targetIndicator.transform.SetParent(allyTransform, true);
        //targetIndicator.transform.localPosition = Vector3.zero;
    }

    public void MoveIndicatorToEntity(Entity entity)
    {
        if (targetIndicator != null && entity != null)
        {
            StartCoroutine(SmoothMoveIndicator(entity.transform));
            currentTargetedEntity = entity;
            moving = true;
        }
    }
}
