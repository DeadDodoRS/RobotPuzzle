using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton
    private static CameraController gameController;

    public static CameraController Instance()
    {
        if (gameController == null)
            gameController = FindObjectOfType<CameraController>();

        return gameController;
    }
    #endregion

    [SerializeField] private float transitionDuration = 2.5f;
    [SerializeField] private float deltaXInGameMode = 1.5f;
    private Vector3 target;

    public void SetGameMode()
    {
        target = new Vector3(transform.position.x - deltaXInGameMode, transform.position.y, transform.position.z);
        StartCoroutine(Transition());
    }

    public void SetMenuMode()
    {
        target = new Vector3(transform.position.x + deltaXInGameMode, transform.position.y, transform.position.z);
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

    }
}
