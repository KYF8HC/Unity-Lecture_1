using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveItems : MonoBehaviour
{
    private Dictionary<GameObject, Vector3> defaultTransforms;
    private List<Transform> allChildren;
    private float animationDuration = 5f;

    private void Awake()
    {
        allChildren = new List<Transform>();
        defaultTransforms = new Dictionary<GameObject, Vector3>();
    }

    private void Start()
    {
        GetAllChildren(transform);
        foreach (var child in allChildren)
        {
            defaultTransforms.Add(child.gameObject, child.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var child in allChildren)
            {
                StartCoroutine(SwapTransforms(child, new Vector3(Random.Range(0, 10), 1, Random.Range(0, 10))));
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var child in allChildren)
            {
                StartCoroutine(SwapTransforms(child, defaultTransforms[child.gameObject]));
            }
        }
    }

    private IEnumerator SwapTransforms(Transform transform, Vector3 targetPosition)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        while (Time.time - startTime < animationDuration)
        {
            float t = (Time.time - startTime) / animationDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
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