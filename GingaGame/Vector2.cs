using System;

namespace GingaGame;

public struct Vector2(float x, float y)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;

    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.X * b, a.Y * b);
    }

    public static Vector2 operator *(float a, Vector2 b)
    {
        return new Vector2(a * b.X, a * b.Y);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(a.X / b, a.Y / b);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator +(Vector2 a, float b)
    {
        return new Vector2(a.X + b, a.Y + b);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator -(Vector2 a)
    {
        return new Vector2(-a.X, -a.Y);
    }

    private float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }

    public static int Distance(Vector2 a, Vector2 b)
    {
        return (int)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }

    public static int Normalize(Vector2 a)
    {
        return (int)Math.Sqrt(a.X * a.X + a.Y * a.Y);
    }

    public Vector2 Normalized()
    {
        return this / Magnitude();
    }

    public double Dot(Vector2 normal)
    {
        return X * normal.X + Y * normal.Y;
    }

    public static double Dot(Vector2 a, Vector2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }
}