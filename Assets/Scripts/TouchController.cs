using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public delegate void TouchEventDelegate(int touchIndex, TouchType touchType);

public class TouchController : Singleton<TouchController>
{
	// Fields
	private HashSet<int> activeTouchIDs = new HashSet<int>();
	public static int MAX_TOUCHES = 1;
	public TouchEventDelegate touchEvent;

	private void Update()
	{
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			int fingerId = touch.fingerId;
			//Если палец только прикоснулся и пальцев наэкране меньше чем максималное кол-во
			if ((touch.phase == TouchPhase.Began) && (activeTouchIDs.Count < MAX_TOUCHES))
			{
				//То добавляем этот палец в ХешСет активных пальцев
				activeTouchIDs.Add(fingerId);
			}
			//Иначе если палец есть в хешсете
			else if (!this.activeTouchIDs.Contains(fingerId))
			{
				//Пропустить последующие действия в for и перейти к следющуму этапу цикла
				continue;
			}
			//Если нажатие отменено или законченно
			if ((touch.phase == TouchPhase.Canceled) || (touch.phase == TouchPhase.Ended))
			{
				//Убрать номер пальца из ХешСета
				this.activeTouchIDs.Remove(fingerId);
			}
			//Переводим координаты пальца в координаты мира
			//Vector3 position = Camera.main.ScreenToWorldPoint((Vector3) touch.position);

			TouchType began = TouchType.Began;
			if (touch.phase == TouchPhase.Began)
			{
				began = TouchType.Began;
			}
			else if ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary))
			{
				began = TouchType.Moved;
			}
			else
			{
				began = TouchType.Ended;
			}
			if (touchEvent != null && !TouchedHUD (touch.position))
			{
				touchEvent(fingerId, began);
			}
		}
	}

	private bool TouchedHUD (Vector3 screenPosition)
	{
		PointerEventData eventData = new PointerEventData (EventSystem.current) { position = screenPosition };
		List<RaycastResult> raycastResults = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventData, raycastResults);
		bool flag = false;
		raycastResults.ForEach ((RaycastResult result) => {
			if (result.gameObject.tag != "HideUI" && flag != true)
				flag = true;
		});
		return flag;
	}

	public void TestTap ()
	{
		if (touchEvent != null) {
			touchEvent (1, TouchType.Began);
		}
	}
}

public enum TouchType
{
	Began,
	Moved,
	Ended
}
