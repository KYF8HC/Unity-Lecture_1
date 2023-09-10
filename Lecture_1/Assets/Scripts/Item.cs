using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
    [SerializeField] private MoveItem controller;
    [SerializeField] private float minPosition = 0f;
    [SerializeField] private float maxPosition = 10f;
    [SerializeField] private float minScale = 4f;
    [SerializeField] private float maxScale = 4f;
    
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private Vector3 defaultScale;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;
    
    private float animationDuration = 5f;
    private float startTime;
    private bool isAnimating = false;

    private void Awake()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        controller.OnScrambleAction += MoveItem_OnScrambleAction;
        controller.OnReturnToOrigin += MoveItem_OnReturnToOrigin;
    }

    private void OnDisable()
    {
        controller.OnScrambleAction -= MoveItem_OnScrambleAction;
        controller.OnReturnToOrigin -= MoveItem_OnReturnToOrigin;
    }

    private void Update()
    {
        if (isAnimating)
        {
            float t = (Time.time - startTime) / animationDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            if (t >= 1f)
            {
                isAnimating = false;
            }
        }
    }

    private void MoveItem_OnScrambleAction(object sender, EventArgs e)
    {
        targetPosition = new Vector3(Random.Range(minPosition, maxPosition),
            Random.Range(minPosition, maxPosition), Random.Range(minPosition, maxPosition));
        targetRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        targetScale = new Vector3(Random.Range(minScale, maxScale), Random.Range(minScale, maxScale),
            Random.Range(minScale, maxScale));
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        
        startTime = Time.time;
        isAnimating = true;
    }

    private void MoveItem_OnReturnToOrigin(object sender, EventArgs e)
    {
        targetPosition = defaultPosition;
        targetRotation = defaultRotation;
        targetScale = defaultScale;
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        
        startTime = Time.time;
        isAnimating = true;
    }
}