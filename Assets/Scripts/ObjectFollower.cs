using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
	public bool FollowX, FollowY;
	public GameObject target;

	private void Update()
	{
		Vector2 newPos;
		if (FollowX)
			newPos.x = target.transform.position.x;
		else
			newPos.x = transform.position.x;
		
		if (FollowY)
			newPos.y = target.transform.position.y;
		else
			newPos.y = transform.position.y;

		transform.position = newPos;
	}
}
