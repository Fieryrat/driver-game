using UnityEngine;
using System.Collections;

public class PackObject : MonoBehaviour
{


	[SerializeField]
	float xPosition;
	[SerializeField]
	float distanceBetweenCars = 63f;
	[SerializeField]
 	CoinsPackSelectorController controller;

	public string sku = "";
	public string info = "";

	private float zoom = 0f;
	private bool lastBoolZoom;

	void Update ()
	{
		bool flag = Mathf.Abs ((-xPosition * distanceBetweenCars) - (this.transform.parent.localPosition.x)) < (distanceBetweenCars / 2f);

		if (flag && (this.lastBoolZoom != flag)) {
			controller.SetCenterObject (this);
		}
		this.lastBoolZoom = flag;

		if (flag && (this.zoom < 1f)) {
			this.zoom += Time.deltaTime * 6f;
			if (this.zoom >= 1f) {
				this.zoom = 1f;
			}
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1f + (0.5f * zoom), 1f + (0.5f * zoom), 1f);
		} else if (!flag && (this.zoom > 0f)) {
			this.zoom -= Time.deltaTime * 6f;
			if (this.zoom <= 0f) {
				//gameObject.GetComponent<Transform> ().localPosition = standartPos;
				this.zoom = 0f;
			}
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1f + (0.5f * zoom), 1f + (0.5f * zoom), 1f);
		}

	}

}
