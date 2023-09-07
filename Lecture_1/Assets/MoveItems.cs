using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveItems : MonoBehaviour
{
    private Dictionary<GameObject, Transform> defaultTransforms;
    private List<Transform> allChildren;
    private float animationDuration = 5f;

    private void Awake()
    {
        allChildren = new List<Transform>();
        defaultTransforms = new Dictionary<GameObject, Transform>();
    }

    private void Start()
    {
        GetAllChildren(transform);
        foreach (var child in allChildren)
        {
            defaultTransforms.Add(child.gameObject, child);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var child in allChildren)
            {
                var targetPosition = new Vector3(Random.Range(0, 10), 1, Random.Range(0, 10));
                var targetRotation =
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                var targetLocalScale = new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), Random.Range(0f, 2f));
                StartCoroutine(SwapTransforms(child, targetPosition, targetRotation, targetLocalScale));
            }

            foreach (var child in allChildren)
            {
                var defaultTransform = defaultTransforms[child.gameObject];
                print(child.gameObject.name + ": " + defaultTransform.position + " " + defaultTransform.rotation +
                      ", " + defaultTransform.localScale);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var child in allChildren)
            {
                var defaultTransform = defaultTransforms[child.gameObject];
                StartCoroutine(SwapTransforms(child, defaultTransform.position, defaultTransform.rotation,
                    defaultTransform.localScale));
            }
        }
    }

    private IEnumerator SwapTransforms(Transform childTransform, Vector3 targetPosition, Quaternion targetRotation,
        Vector3 targetScale)
    {
        float startTime = Time.time;
        while (Time.time - startTime < animationDuration)
        {
            float t = (Time.time - startTime) / animationDuration;
            childTransform.position = Vector3.Lerp(childTransform.position, targetPosition, t);
            childTransform.rotation = Quaternion.Lerp(childTransform.rotation, targetRotation, t);
            childTransform.localScale = Vector3.Lerp(childTransform.localScale, targetScale, t);
            yield return null;
        }
    }

    private void GetAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            allChildren.Add(child);
            GetAllChildren(child);
        }
    }
}