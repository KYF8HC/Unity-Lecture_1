using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveItems : MonoBehaviour
{
    public struct TransformValues
    {
        public Vector3 defaultPosition;
        public Quaternion defaultRotation;
        public Vector3 defaultScale;
    }

    private Dictionary<GameObject, TransformValues> defaultTransforms;
    private List<Transform> allChildren;
    private List<Transform> removeFromList;
    private float animationDuration = 5f;

    private void Awake()
    {
        allChildren = new List<Transform>();
        removeFromList = new List<Transform>();
        defaultTransforms = new Dictionary<GameObject, TransformValues>();
    }

    private void Start()
    {
        GetAllChildren(transform);
        foreach (var child in allChildren)
        {
            var childValues = new TransformValues();
            childValues.defaultPosition = child.position;
            childValues.defaultRotation = child.rotation;
            childValues.defaultScale = child.localScale;
            defaultTransforms.Add(child.gameObject, childValues);
        }
        foreach (var child in allChildren)
        {
            if(child.gameObject.name == "Reflection Probes")
            {
                removeFromList.Add(child);
            }
            if (child.GetComponent<ReflectionProbe>() != null)
            {
                removeFromList.Add(child);
            }
            if(child.GetComponent<LightProbeGroup>() != null)
            {
                removeFromList.Add(child);
            }
        }

        foreach (var child in removeFromList)
        {
            if (allChildren.Contains(child))
            {
                allChildren.Remove(child);
            }
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var child in allChildren)
            {
                var targetPosition = new Vector3(Random.Range(0, 10), Random.Range(0, 4), Random.Range(0, 10));
                var targetRotation =
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                var targetScale = new Vector3(Random.Range(0f, 4f), Random.Range(0f, 4f), Random.Range(0f, 4f));
                StartCoroutine(SwapTransforms(child, child.position, child.rotation, child.localScale, targetPosition,
                    targetRotation, targetScale));
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var child in allChildren)
            {
                var defaultTransform = defaultTransforms[child.gameObject];
                StartCoroutine(SwapTransforms(child, child.position, child.rotation, child.localScale,
                    defaultTransform.defaultPosition, defaultTransform.defaultRotation,
                    defaultTransform.defaultScale));
            }
        }
    }

    private IEnumerator SwapTransforms(Transform childTransform, Vector3 startPosition, Quaternion startRotation,
        Vector3 startScale, Vector3 targetPosition, Quaternion targetRotation,
        Vector3 targetScale)
    {
        float startTime = Time.time;
        while (Time.time - startTime < animationDuration)
        {
            float t = (Time.time - startTime) / animationDuration;
            childTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            childTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            childTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
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