using System;
using UnityEngine;

class CustomRay : IComparable<CustomRay>
{
    private Vector3 _position { get; set; }
    private Vector3 _direction { get; set; }
    private Wall _wall { get; set; }

    public Vector3 origin { get => _position; }
    public Vector3 direction { get => _direction; }
    public Wall wall { get => _wall; }

    public CustomRay() { }

    public CustomRay SetRayParams(Vector3 position, Vector3 direction)
    {
        this._position = position;
        this._direction = Vector3.Normalize(direction);
        return this;
    }

    public void SetWall(Wall wall)
    {
        this._wall = wall;
    }

    public int CompareTo(CustomRay otherRay)
    {
        float thisAngle = ConvertPositiveAngle(this);
        float OtherAngle = ConvertPositiveAngle(otherRay);

        if (thisAngle > OtherAngle)
            return 1;
        else if (thisAngle < OtherAngle)
            return -1;
        else
            return 0;
    }

    private float ConvertPositiveAngle(CustomRay ray)
    {
        float absAngle = Vector3.Angle(Vector3.up, ray.direction);
        if (ray.direction.x < 0)
        {
            return 360 - absAngle;
        }
        return absAngle;
    }
}