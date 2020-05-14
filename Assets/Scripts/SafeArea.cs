using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
	RectTransform Panel;
	Rect lastSafeArea = new Rect(0, 0, 0, 0);

	private void Awake()
	{
		Panel = GetComponent<RectTransform>();
		Refresh();
	}
    // Update is called once per frame
    void Update()
    {
		Refresh();
    }

	void Refresh()
	{
		Rect safeArea = GetSafeArea();

		if (safeArea != lastSafeArea)
		{
			ApplySafeArea(safeArea);
		}
	}

	Rect GetSafeArea()
	{
		return Screen.safeArea;
	}

	void ApplySafeArea(Rect rect)
	{
		lastSafeArea = rect;

		Vector2 anchorMin = rect.position;
		Vector2 anchorMax = rect.position + rect.size;
		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;
		Panel.anchorMin = anchorMin;
		Panel.anchorMax = anchorMax;
	}
}
