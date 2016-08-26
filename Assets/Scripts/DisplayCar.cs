using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayCar : MonoBehaviour {

	public float xPosition;
	public float zoom = 0f;
	[SerializeField] float distanceBetweenCars = 63f;
	public Car carInfo;
	private bool lastBoolZoom;
	private CarsSelectionController carsSelectionController;
	private Vector3 standartPos;


	
	public void SetCar (Car currentcar, CarsSelectionController controller) {
		if (currentcar != null) {
			carInfo = currentcar;
			gameObject.GetComponent<Image> ().sprite = currentcar.carSprite;
			RefreshCarCoolor ();
		} else {
			carInfo = null;
		}

		carsSelectionController = controller;
	}

	public void RefreshCarCoolor(){
		if (!carInfo.unlocked)
			gameObject.GetComponent<Image> ().color = GameConstants.CARSPRITELOCK;
		else
			gameObject.GetComponent<Image> ().color = Color.white;
	}
		

	void Update()
	{
		bool flag = Mathf.Abs((-xPosition * distanceBetweenCars) - (this.transform.parent.localPosition.x)) < (distanceBetweenCars/2f);

		if (flag && (this.lastBoolZoom != flag))
		{
			carsSelectionController.SetCenterCar(this);
			//standartPos = gameObject.GetComponent<Transform> ().localPosition;
		}
		this.lastBoolZoom = flag;

		if (flag && (this.zoom < 1f))
		{
			this.zoom += Time.deltaTime * 6f;
			if (this.zoom >= 1f)
			{
				this.zoom = 1f;
			}
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1f + (0.5f * zoom), 1f + (0.5f * zoom), 1f);
		}
		else if (!flag && (this.zoom > 0f))
		{
			this.zoom -= Time.deltaTime * 6f;
			if (this.zoom <= 0f)
			{
				//gameObject.GetComponent<Transform> ().localPosition = standartPos;
				this.zoom = 0f;
			}
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1f + (0.5f * zoom), 1f + (0.5f * zoom), 1f);
		}

	}


}
