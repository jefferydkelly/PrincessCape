using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void WaitDelegate();
public static class ExtensionMethods{

	/// <summary>
	/// Checks if the tag of the Collider's objects contains the given string
	/// </summary>
	/// <returns><c>true</c>, if the Collider's tag contains the string, <c>false</c> otherwise.</returns>
	/// <param name="col">Col.</param>
	/// <param name="tag">Tag.</param>
	public static bool TagContains(this Collider2D col, string tag)
	{
		
		return col.tag.Contains(tag);
	}

    public static bool OnLayer(this Collider2D col, string layer)
    {
        return col.gameObject.layer == LayerMask.NameToLayer(layer);
    }

	public static float HalfWidth(this SpriteRenderer sr) {
		return sr.bounds.extents.x;
	}

	public static float HalfHeight(this SpriteRenderer sr)
	{
		return sr.bounds.extents.y;
	}

	public static float HalfWidth(this Collider2D c)
	{
		return c.bounds.extents.x;
	}

	public static float HalfHeight(this Collider2D c)
	{
		return c.bounds.extents.y;
	}

	public static float HalfWidth(this GameObject go)
	{
		SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

		if (sr)
		{
			return sr.HalfWidth();
		}

		Collider2D col = go.GetComponent<Collider2D>();

		if (col)
		{
			return col.HalfWidth();
		}

		return go.transform.localScale.x / 2;
	}

	public static float HalfHeight(this GameObject go)
	{
		SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

		if (sr)
		{
			return sr.HalfHeight();
		}

		Collider2D col = go.GetComponent<Collider2D>();

		if (col)
		{
			return col.HalfHeight();
		}

		return go.transform.localScale.y / 2;
	}

    public static bool IsLeftOf(this GameObject g, GameObject go)
    {
        Vector3 dif = (go.transform.position - g.transform.position).normalized;
        float dot = Vector3.Dot(Vector3.left, dif);
        return dot >  1.0f / Mathf.Sqrt(2);
    }

    public static bool IsRightOf(this GameObject g, GameObject go)
    {
        return go.IsLeftOf(g);
    }
    public static bool IsAbove(this GameObject g, GameObject go)
	{
		Vector3 dif = (go.transform.position - g.transform.position).normalized;
		float dot = Vector3.Dot(Vector3.down, dif);
        return dot > 1.0f / Mathf.Sqrt(2);
    }

	public static bool IsBelow(this GameObject g, GameObject go)
	{
		return go.IsAbove(g);
	}

	public static void Remove<T>(this List<T> li, List<T> list)
	{
		if (li.GetType() == list.GetType())
		{
			foreach (T i in list)
			{
				li.Remove(i);
			}
		}
	}

	public static bool IsEmpty<T>(this List<T> li) {
		return li.Count == 0;
	}

	public static void AddExclusive<T>(this List<T> li, T item) {
		if (!li.Contains (item)) {
			li.Add (item);
		}
	}

	public static T RandomElement<T>(this List<T> l)
	{
		return l[Mathf.RoundToInt(Random.value * l.Count)];
	}

	public static Rect SizeRect(this Texture t)
	{
		return new Rect(0, 0, t.width, t.height);
	}

	public static Vector2 Center(this Texture t)
	{
		return new Vector2(t.width, t.height) / 2;
	}

	public static void ClampVelocity(this Rigidbody2D rb, float maxSpeed, VelocityType vt = VelocityType.Full)
	{
		if (vt == VelocityType.Full)
		{
			if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
			{
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}
		}
		else if (vt == VelocityType.X)
		{

			if (Mathf.Abs(rb.velocity.x) > maxSpeed)
			{
				rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
			}
		}
		else if (vt == VelocityType.Y)
		{
			if (Mathf.Abs(rb.velocity.y) > maxSpeed)
			{
				rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxSpeed);
			}
		}
	}

	public static void ReverseGravity(this Rigidbody2D rb)
	{
		rb.gravityScale *= -1;
	}

    public static void Translate(this Rigidbody2D rb, Vector2 v)
    {
        rb.MovePosition((Vector2)rb.transform.position + v);
    }

	public static bool BetweenEx(this float f, float min, float max)
	{
		return f > min && f < max;
	}

	public static bool Between(this float f, float min, float max)
	{
		return f >= min && f <= max;
	}

    public static float ToRadians(this float f)
    {
		float ang = f * Mathf.Deg2Rad;
		while (ang < 0) {
			ang += Mathf.PI * 2;
		}
		return ang;
    }

    public static float ToDegrees(this float f)
    {
		float ang = f * Mathf.Rad2Deg;
		while (ang < 0) {
			ang += 360;
		}
		return ang;
    }

	public static Vector2 FromDegToVec2(this float f) {
		float rad = f.ToRadians ();
		return new Vector2 (Mathf.Cos (rad), Mathf.Sin (rad));
	}

	public static Vector2 FromRadToVec2(this float rad) {
		return new Vector2 (Mathf.Cos (rad), Mathf.Sin (rad));
	}

	public static string ToXString(this Vector2 v)
	{
		return v.x + "x" + v.y;
	}

    public static Vector2 XVector(this Vector2 v)
    {
        return new Vector2(v.x, 0);
    }
    public static Vector2 YVector(this Vector2 v)
    {
        return new Vector2(0, v.y);
    }

	public static bool VectorsEqual(this Vector2 v1, Vector2 v2) {
		return v1.x == v2.x && v1.y == v2.y;
	}

	/// <summary>
	/// Rotate the specified Vector2 by the given angle.
	/// </summary>
	/// <param name="v">V.</param>
	/// <param name="angle">Angle.</param>
	public static void Rotate(this Vector2 v, float angle) {
		float dx = v.x;
		float dy = v.y;
		float sin = Mathf.Sin (angle);
		float cos = Mathf.Cos (angle);
		v.x = dx * cos - dy * sin;
		v.y = dx * sin + dy * cos;
	}

	public static  Vector2 Rotated(this Vector2 v, float angle) {
		float sin = Mathf.Sin (angle);
		float cos = Mathf.Cos (angle);
		return new Vector2 (v.x * cos - v.y * sin, v.x * sin + v.y * cos);
	}

	public static float GetAngle(this Vector2 v) {
		float ang = Mathf.Atan2 (v.y, v.x);
        while(ang < 0)
        {
            ang += Mathf.PI * 2;
        }
        return ang;
	}

	public static float Dot(this Vector2 v1, Vector2 v2) {
		return v1.x * v2.x + v1.y * v2.y;
	}

    public static string ToXString(this Vector3 v)
	{
		return v.x + "x" + v.y + "x" + v.z;
	}

    public static Vector3 XVector(this Vector3 v)
    {
        return new Vector3(v.x, 0);
    }
    public static Vector3 YVector(this Vector3 v)
    {
        return new Vector3(0, v.y);
    }

    public static Vector3 ZVector(this Vector3 v)
    {
        return new Vector3(0, 0, v.z);
    }

	public static bool VectorsEqual(this Vector3 v1, Vector3 v2) {
		return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
	}
}

public enum VelocityType
{
	X, Y, Full
}
