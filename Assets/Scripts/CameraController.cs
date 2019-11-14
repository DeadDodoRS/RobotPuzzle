using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MBSingleton<CameraController>
{

    [SerializeField] private float transitionDuration = 2.5f;
    [SerializeField] private Vector3 InGameCameraDelta;
    private Vector3 target;

    public void SetGameMode()
    {
        target = transform.position + InGameCameraDelta;
        StartCoroutine(Transition());
    }

    public void SetMenuMode()
    {
        target = transform.position - InGameCameraDelta;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            transform.position = Vector3.Lerp(startingPos, target, t);
            yield return 0;
        }

        transform.position = target;
    }
}
