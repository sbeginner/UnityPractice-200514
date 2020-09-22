using System.Collections.Generic;

using UnityEngine;

public class RayCast : MonoBehaviour
{
    private GameManager gameManager;
    private Camera _mainCamera;
    private Vector3 mousePosition;
    private bool _init;
    public bool preloadIsComplete { get => _init; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.SetRayCast(this);

        _mainCamera = Camera.main;

        // Finish init code
        _init = true;
    }

    private void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f;
        mousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        if (!Input.GetMouseButton(0))
        {
            float step = .5f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, step);
        }
    }

    public void OnDrawGizmos()
    {
        if (!_init)
        {
            return;
        }

        drawRaySpot();
        raySearch();
    }

    private void drawRaySpot()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position, .15f);
    }

    // Start a single ray to search all the potential direction "in order"
    private void raySearch()
    {
        CustomRay singleRay = new CustomRay();
        Vector3 firstCastPointOnWall = Vector3.negativeInfinity;
        Vector3 initCastPointOnWall = firstCastPointOnWall;
        Vector3 prevCastPoints = Vector3.negativeInfinity;

        foreach (CustomRay ray in createCustomRays())
        {
            singleRay.SetRayParams(transform.position, ray.direction);

            (Wall wall, Vector3 currentCastPoints) = RayCastToWall(singleRay);
            if (wall == null) continue;

            // Draw line and point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firstCastPointOnWall, .12f);
            Gizmos.DrawLine(currentCastPoints, prevCastPoints);

            // TODO: Optimize the code
            // Store the init cast point
            if (initCastPointOnWall.Equals(Vector3.negativeInfinity))
            {
                initCastPointOnWall = firstCastPointOnWall;
            }

            // Move to another wall (This is the critical part)
            if (singleRay.wall != wall || firstCastPointOnWall.Equals(Vector3.negativeInfinity))
            {
                singleRay.SetWall(wall);
                firstCastPointOnWall = currentCastPoints;
            }
            prevCastPoints = currentCastPoints;
        }
        Gizmos.DrawLine(initCastPointOnWall, prevCastPoints);
    }

    // Prepare All the rays and sorted by direction
    private List<CustomRay> createCustomRays()
    {
        List<CustomRay> rayList = new List<CustomRay>();

        gameManager.GetRectangles().ForEach((rect) =>
         {
             foreach (Wall wall in rect.walls)
             {
                 Vector3 dir = wall.wallPointStart - transform.position;

                 rayList.Add(
                     new CustomRay().SetRayParams(
                         transform.position,
                         Quaternion.AngleAxis(.05f, Vector3.forward) * dir));

                 rayList.Add(
                     new CustomRay().SetRayParams(
                         transform.position,
                         Quaternion.AngleAxis(-.05f, Vector3.forward) * dir));
             }
         });

        rayList.Sort();

        return rayList;
    }

    // Find the nearest cast point from all the walls for a ray
    private (Wall, Vector3) RayCastToWall(CustomRay ray)
    {
        Wall nearestWall = null;
        Vector3 nearestCastPoint = Vector3.negativeInfinity;
        float minDist = float.MaxValue;

        gameManager.GetRectangles().ForEach((rect) =>
        {
            foreach (Wall wall in rect.walls)
            {
                // Find the ray can cast on the wall (segment) or not, ignore distance
                Vector3 castPoint = FindWallCastSpot(
                    ray.origin, ray.origin + ray.direction,
                    wall.wallPointStart, wall.wallPointEnd);

                // Find nothing
                if (castPoint.Equals(Vector3.negativeInfinity)) continue;

                // Find the nearest wall in this current ray
                float distance = Vector3.Distance(transform.position, castPoint);
                if (minDist > distance)
                {
                    minDist = distance;
                    nearestCastPoint = castPoint;
                    nearestWall = wall;
                }
            }
        });
        return (nearestWall, nearestCastPoint);
    }

    // Line -Line Intersection algorithm (Math)
    private Vector3 FindWallCastSpot(
        Vector3 rayPointStart, Vector3 rayPointEnd,
        Vector3 wallPointStart, Vector3 wallPointEnd)
    {
        float x1 = wallPointStart.x;
        float y1 = wallPointStart.y;
        float x2 = wallPointEnd.x;
        float y2 = wallPointEnd.y;

        float x3 = rayPointStart.x;
        float y3 = rayPointStart.y;
        float x4 = rayPointEnd.x;
        float y4 = rayPointEnd.y;

        float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (den == 0)
            return Vector3.negativeInfinity;

        float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
        float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

        if (t > 0 && t < 1 && u > 0)
        {
            Vector3 castPoint = new Vector3(x1 + t * (x2 - x1), y1 + t * (y2 - y1), 0);
            return castPoint;
        }
        return Vector3.negativeInfinity;
    }
}


