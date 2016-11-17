using UnityEngine;
using System.Collections;

public class TileBrush : MonoBehaviour {

	public Vector2 size = Vector2.zero;
	public int tileID = 0;
	public SpriteRenderer renderer2D;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireCube(transform.position, size);
	}

	public void UpdateBrush(Sprite sprite)
	{
		renderer2D.sprite = sprite;
	}
}
