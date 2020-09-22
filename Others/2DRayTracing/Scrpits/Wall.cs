using System;
using UnityEngine;

public class Wall : IEquatable<Wall>
{
    public Vector3 wallPointStart { get; private set; }
    public Vector3 wallPointEnd { get; private set; }

    public Wall(Vector3 wallPointStart, Vector3 wallPointEnd)
    {
        _Init(wallPointStart, wallPointEnd);
    }

    private void _Init(Vector3 wallPointStart, Vector3 wallPointEnd)
    {
        this.wallPointStart = wallPointStart;
        this.wallPointEnd = wallPointEnd;
    }

    public Wall SetWallPoint(Vector3 wallPointStart, Vector3 wallPointEnd)
    {
        _Init(wallPointStart, wallPointEnd);
        return this;
    }

    public static bool operator ==(Wall w1, Wall w2)
    {
        if ((System.Object)w1 == (System.Object)w2)
        {
            return true;
        }

        if ((System.Object)w1 == null || (System.Object)w2 == null)
        {
            return false;
        }

        return w1.Equals(w2);
    }

    public static bool operator !=(Wall b1, Wall b2) => !(b1 == b2);

    public override bool Equals(System.Object obj) => obj is Wall o && Equals(o);

    public bool Equals(Wall other)
    {
        return (this.wallPointStart == other.wallPointStart) &&
                      (this.wallPointEnd == other.wallPointEnd);
    }

    // Auto-Generate
    public override int GetHashCode()
    {
        int hashCode = 705050641;
        hashCode = hashCode * -1521134295 + wallPointStart.GetHashCode();
        hashCode = hashCode * -1521134295 + wallPointEnd.GetHashCode();
        return hashCode;
    }
}
