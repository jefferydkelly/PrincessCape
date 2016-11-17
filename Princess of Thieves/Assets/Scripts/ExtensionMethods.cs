using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void WaitDelegate();
public static class ExtensionMethods{
	
	public static bool TagContains(this Collider2D col, string tag)
	{
		
		return col.tag.Contains(tag);
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

	public static bool IsAbove(this GameObject g, GameObject go)
	{
		Vector3 dif = (go.transform.position - g.transform.position).normalized;
		float dot = Vector3.Dot(Vector3.down, dif);
		return dot > 0;
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

	public static IEnumerator RunAfter(this GameObject go, WaitDelegate w, float time)
	{
		float dt = 0;
		while (dt < time)
		{
			if (!GameManager.Instance.IsPaused)
			{
				dt += Time.deltaTime;
			}
			yield return null;

		}
		w();
	}

	public static bool BetweenEx(this float f, float min, float max)
	{
		return f > min && f < max;
	}

	public static bool Between(this float f, float min, float max)
	{
		return f >= min && f <= max;
	}

	public static string ToXString(this Vector2 v)
	{
		return v.x + "x" + v.y;
	}

	public static string ToXString(this Vector3 v)
	{
		return v.x + "x" + v.y + "x" + v.z;
	}
}

public enum VelocityType
{
	X, Y, Full
}
