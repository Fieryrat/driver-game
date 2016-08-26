using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CoinsPackSelectorController : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
	
	[SerializeField]
	Text textSelectInfo;
	[SerializeField]
	ShopButton buttonScript;
	[SerializeField]
	Transform objectsDisplayAnchor;
	[SerializeField]
	float toCenter = 117f;

	private PackObject centerObject;
	private bool dragging = false;
	private float draggingX = 0;

	private float lastClack;

	public float currentVel = 0; //Текущая скорость скорллинга до машины.

	public void SetCenterObject (PackObject displayPack)
	{
		centerObject = displayPack;

		textSelectInfo.text = displayPack.info;

		//Клик
		if (lastClack <= 0f) {
			//Проигрывание клика при пролистывании
			//SoundEmitter.instance.GetComponent<AudioSource>().PlayOneShot(this.soundClack);
			this.lastClack = 0.05f;
		}
	}

	private void Update ()
	{
		if (lastClack > 0f) {
			lastClack -= Time.deltaTime;
		}

		if (dragging) {
			float _x = Mathf.SmoothDamp (objectsDisplayAnchor.localPosition.x, draggingX, ref currentVel, 0.9f);

			objectsDisplayAnchor.localPosition = new Vector3 (_x, objectsDisplayAnchor.localPosition.y, 0);
			dragging = !Mathf.Approximately (draggingX, objectsDisplayAnchor.localPosition.x);
		}
	}

	public void OnEndDrag (PointerEventData data)
	{
		draggingX = -(centerObject.gameObject.transform.localPosition.x - toCenter);
		dragging = true;
	}

	public void OnBeginDrag (PointerEventData data)
	{
		dragging = false;
	}
}
