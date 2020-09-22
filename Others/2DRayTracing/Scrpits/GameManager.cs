using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Rectangle> recSubjects;
    private RayCast raySpot;
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get => _instance ?? new GameManager();
    }

    GameManager()
    {
        recSubjects = new List<Rectangle>();
        _instance = this;
    }

    private void Start()
    {
        // TODO: Write a create rectangles function
        new Rectangle(new Vector3(1, 1, 0), new Vector3(1.5f, 1.5f, 0), 0);
        new Rectangle(new Vector3(5, 1, 0), new Vector3(2.5f, 3.5f, 0), 25);
        new Rectangle(new Vector3(3, 2, 0), new Vector3(1, 1, 0), 75, true);
        new Rectangle(new Vector3(0, 0, 0), new Vector3(8, 8, 0), 0);
    }

    private void OnDrawGizmos()
    {
        if (recSubjects.Count > 0 && recSubjects != null)
        {
            recSubjects.ForEach(rec => rec.OnDrawGizmos());
            raySpot.OnDrawGizmos();
        }
    }

    public void SetRayCast(RayCast raySpot)
    {
        this.raySpot = raySpot;
    }

    public void AddRectangle(Rectangle rect)
    {
        recSubjects.Add(rect);
    }

    public void RemoveRectangle(Rectangle rect)
    {
        recSubjects.Remove(rect);
    }

    public List<Rectangle> GetRectangles()
    {
        return recSubjects;
    }
}
