using System.Collections;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    // Enum to define the behavior when reaching the end of the path
    public enum PathEndAction
    {
        Loop,    // Default: Loop to the first route and continue
        Reverse, // Reverse the path and go back to the start
        Stop     // Stop at the end of the path
    }

    [SerializeField] private Transform[] routes;  // Array of routes with control points
    public PathEndAction endAction = PathEndAction.Loop;  // Set the behavior at the end of the path
    public float speedModifier = 0.5f;  // Speed at which the object moves

    private int routeToGo;
    private float tParam;
    private Vector2 objectPosition;
    private bool coroutineAllowed;
    private bool isReversing = false;  // Flag for reversing the path

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
    }

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        // Get the control points for the current route
        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        // Move along the Bezier curve
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            if (isReversing)
            {
                objectPosition = Mathf.Pow(tParam, 3) * p0 +
                              3 * Mathf.Pow(tParam, 2) * (1 - tParam) * p1 +
                              3 * tParam * Mathf.Pow(1 - tParam, 2) * p2 +
                              Mathf.Pow(1 - tParam, 3) * p3;
            }
            else
            {
                objectPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                              3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                              3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                              Mathf.Pow(tParam, 3) * p3;
            }

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        switch (endAction)
        {
            case PathEndAction.Loop:
                routeToGo += 1;
                if (routeToGo > routes.Length - 1)
                {
                    routeToGo = 0;
                }
                break;

            case PathEndAction.Reverse:
                isReversing = !isReversing;

                if (isReversing)
                {
                    routeToGo -= 1;
                    if (routeToGo < 0)
                    {
                        routeToGo = routes.Length - 1;
                    }
                }
                else
                {
                    routeToGo += 1;
                    if (routeToGo > routes.Length - 1)
                    {
                        routeToGo = 0;
                    }
                }
                break;

            case PathEndAction.Stop:
                coroutineAllowed = false;
                yield break;
        }

        coroutineAllowed = true;
    }
}
