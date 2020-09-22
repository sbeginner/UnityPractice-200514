using System.Collections.Generic;
using UnityEngine;

public class Rectangle
{
    private GameManager _gameManager;
    private Vector3[] _points;
    private float _prevDegree;
    private Quaternion _quat;
    private bool _init;

    public float degree;
    public Vector3 centerPoint;
    public Vector3 size;
    public bool isAutoTurn;
    public bool preloadIsComplete { get => _init; }
    public List<Wall> walls
    {
        get
        {
            List<Wall> walls = new List<Wall>();
            for (int i = 0; i < _points.Length; i++)
            {
                walls.Add(new Wall(_points[i], _points[(i + 1) % _points.Length]));
            }
            return walls;
        }
    }

    public Rectangle(Vector3 centerPoint, Vector3 size, float degree, bool canAutoTurn = false)
    {
        this.centerPoint = centerPoint;
        this.size = size;
        this.degree = degree;
        this.isAutoTurn = canAutoTurn;

        _prevDegree = 0;

        _gameManager = GameManager.Instance;
        _gameManager.AddRectangle(this);

        InitPoints();

        // Finish init code
        _init = true;
    }

    private void InitPoints()
    {
        if (_points == null)
            _points = new Vector3[4];

        // Draw Rectangle
        _points[0] = new Vector3(centerPoint.x - size.x * .5f, centerPoint.y + size.y * .5f, 0);
        _points[1] = new Vector3(centerPoint.x - size.x * .5f, centerPoint.y - size.y * .5f, 0);
        _points[2] = new Vector3(centerPoint.x + size.x * .5f, centerPoint.y - size.y * .5f, 0);
        _points[3] = new Vector3(centerPoint.x + size.x * .5f, centerPoint.y + size.y * .5f, 0);
    }

    public void OnDrawGizmos()
    {
        if (!_init)
        {
            return;
        }

        detectedDegreeChange();

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(centerPoint, .05f);

        for (int i = 0; i < _points.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(_points[i], _points[(i + 1) % _points.Length]);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_points[i], .1f);
        }

        if (isAutoTurn)
            this.degree = (this.degree + .1f) % 360;
    }

    private void detectedDegreeChange()
    {
        if (this._prevDegree == this.degree)
        {
            return;
        }
        
        InitPoints();
        this._prevDegree = this.degree;

        // TODO: Use another algorism without offset
        _quat = Quaternion.AngleAxis(this.degree, Vector3.forward);
        for (int i = 0; i < _points.Length; i++)
            _points[i] = _quat * _points[i];

        Vector3 centerOffset = centerPoint - new Vector3(
            (_points[0] + _points[2]).x * .5f, (_points[1] + _points[3]).y * .5f, 0);
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] += centerOffset;
        }
    }
}
