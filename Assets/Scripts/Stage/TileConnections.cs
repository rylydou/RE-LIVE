using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public struct TileConnections
{
#if UNITY_EDITOR
	[AllowNesting, OnValueChanged("SetAll")]
	[SerializeField] bool all;
#endif
	[Space]
	public bool top;
	public bool topRight;
	public bool right;
	public bool bottomRight;
	public bool bottom;
	public bool bottomLeft;
	public bool left;
	public bool topLeft;

	#region Utils
	public bool GetConnection(int x, int y)
	{
		switch (y)
		{
			case 1:
				switch (x)
				{
					case 1:
						return topRight;
					case 0:
						return top;
					case -1:
						return topLeft;
				}
				break;
			case 0:
				switch (x)
				{
					case 1:
						return right;
					case 0:
						Debug.LogError("Why is there a center tile!?!?!?");
						break;
					case -1:
						return left;
				}
				break;
			case -1:
				switch (x)
				{
					case 1:
						return bottomRight;
					case 0:
						return bottom;
					case -1:
						return bottomLeft;
				}
				break;
		}

		Debug.LogError("Out of range!");

		return false;
	}

	public bool GetConnection(Vector2Int position)
	{
		return GetConnection(position.x, position.y);
	}

	public void SetConnection(int x, int y, bool state)
	{
		// Debug.Log($"{x}, {y} > {state}");

		switch (y)
		{
			case 1:
				switch (x)
				{
					case 1:
						topRight = state;
						break;
					case 0:
						top = state;
						break;
					case -1:
						topLeft = state;
						break;
				}
				break;
			case 0:
				switch (x)
				{
					case 1:
						right = state;
						break;
					case 0:
						Debug.LogError("Why is there a center tile!?!?!?");
						break;
					case -1:
						left = state;
						break;
				}
				break;
			case -1:
				switch (x)
				{
					case 1:
						bottomRight = state;
						break;
					case 0:
						bottom = state;
						break;
					case -1:
						bottomLeft = state;
						break;
				}
				break;
		}

		Debug.LogError($"Out of range at {x}, {y} > {state}!");
	}

	public void SetConnection(Vector2Int position, bool state)
	{
		SetConnection(position.x, position.y, state);
	}
	#endregion

#if UNITY_EDITOR
	void SetAll()
	{
		top = all;
		topRight = all;
		right = all;
		bottomRight = all;
		bottom = all;
		bottomLeft = all;
		left = all;
		topLeft = all;
	}
#endif

	#region Other
	public static bool operator ==(TileConnections left, TileConnections right)
	{
		return left.top == right.top &&
			left.right == right.right &&
			left.bottom == right.bottom &&
			left.left == right.left;
		// return left.Equals(right);
	}

	public static bool operator !=(TileConnections left, TileConnections right)
	{
		return !(left == right);
	}

	public override bool Equals(object obj)
	{
		return obj is TileConnections connections &&
			top == connections.top &&
			topRight == connections.topRight &&
			right == connections.right &&
			bottomRight == connections.bottomRight &&
			bottom == connections.bottom &&
			bottomLeft == connections.bottomLeft &&
			left == connections.left &&
			topLeft == connections.topLeft;
	}

	public override int GetHashCode()
	{
		int hashCode = -162796711;
		hashCode = hashCode * -1521134295 + top.GetHashCode();
		hashCode = hashCode * -1521134295 + topRight.GetHashCode();
		hashCode = hashCode * -1521134295 + right.GetHashCode();
		hashCode = hashCode * -1521134295 + bottomRight.GetHashCode();
		hashCode = hashCode * -1521134295 + bottom.GetHashCode();
		hashCode = hashCode * -1521134295 + bottomLeft.GetHashCode();
		hashCode = hashCode * -1521134295 + left.GetHashCode();
		hashCode = hashCode * -1521134295 + topLeft.GetHashCode();
		return hashCode;
	}
	#endregion
}