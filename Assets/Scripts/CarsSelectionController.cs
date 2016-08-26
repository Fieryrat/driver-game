using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CarsSelectionController : MonoBehaviour, IEndDragHandler, IBeginDragHandler {

	[SerializeField] float speed;

	List<DisplayCar> carsDisplayList;
	public GameObject carDisplayPrefab;

	public Text textSelectName;
	public Text textSelectInfo;
	public ShopButton buttonScript;
	public Transform carDisplayAnchor;

	private DisplayCar centerCar;
	private bool dragging = false;
	private float draggingX = 0;

	private float lastClack;

	public float currentVel = 0; //Текущая скорость скорллинга до машины.

	void Start () {
		carsDisplayList = new List<DisplayCar>();
		List<Car> listCars = Singleton<CarsController>.instance.listCars;
		//this.touchPlane = new Plane(Vector3.forward, Vector3.zero);
		//this.spriteSelectInfo.gameObject.SetActive(false);
		textSelectInfo.gameObject.SetActive(false);

		int num = 0;
		for (int i = 0; i < listCars.Count; i++)
		{
			Car setCar = listCars[i];
			DisplayCar item = UnityEngine.Object.Instantiate<GameObject>(carDisplayPrefab).GetComponent<DisplayCar>();
			item.transform.SetParent(carDisplayAnchor);
			item.transform.localScale = Vector3.one;
			item.name = setCar.carName;
			item.SetCar(setCar, this);
			item.xPosition = num;
			carsDisplayList.Add(item);
			num++;
		}
	}

	void OnEnable ()
	{
		if (carsDisplayList != null)
			carsDisplayList.ForEach ((DisplayCar car) => { car.RefreshCarCoolor (); });
	}

	public void SetCenterCar(DisplayCar displayCar)
	{
		centerCar = displayCar;

		//GlobalController.instance.gameState.CharacterSelected(centerCar.carInfo.carName, 0); //Выбор машины в глобальном скрипте.

		if (centerCar.carInfo.secret) {
			textSelectName.text  = "???";
			textSelectInfo.text = "NOT AVAILABLE AS PRIZE";
			textSelectInfo.gameObject.SetActive (true);
		} else {
			textSelectName.text  = centerCar.carInfo.carName;
			textSelectInfo.gameObject.SetActive (false);
		}

		if (this.centerCar.carInfo.unlocked)
		{
			//Имя белым цветом
			this.textSelectName.color = GameConstants.TEXTCOLOR;
			//Активация кнопки play и выключение показа цены
			buttonScript.ChangeButtonState ("play");
		}
		else
		{
			//Имя серым цветом
			this.textSelectName.color = GameConstants.TEXTCOLORLOCK;

			//Если тачка серетная
			if (centerCar.carInfo.secret) {
				//Включение замочка
				buttonScript.ChangeButtonState ("lock");
			} else {
				//Показ ценника покупки и активация кнопики покупки.
				buttonScript.ChangeButtonState ("buy");
			}
		}


		//Клик
		if (lastClack <= 0f)
		{
			//Проигрывание клика при пролистывании
			//SoundEmitter.instance.GetComponent<AudioSource>().PlayOneShot(this.soundClack);
			this.lastClack = 0.05f;
		}
			

	}

	public void ButtonClick(){
		if (centerCar != null) {
			if (buttonScript.currentState == "play")
				Singleton<CarsController>.instance.SelectCar (centerCar.carInfo);
			else if (buttonScript.currentState == "buy")
				Singleton<CarsController>.instance.BuyCar (centerCar.carInfo);
		}
			
	}

	public void TestClick(){
		if (centerCar != null) {
			centerCar.carInfo.unlocked = true;
			centerCar.RefreshCarCoolor ();
		}
			
	}



	private void Update()
	{
		if (lastClack > 0f)
		{
			lastClack -= Time.deltaTime;
		}

		if (dragging) {

			float _x = Mathf.SmoothDamp (carDisplayAnchor.localPosition.x, draggingX, ref currentVel, speed);

			carDisplayAnchor.localPosition = new Vector3(_x, carDisplayAnchor.localPosition.y, 0);
			dragging = !Mathf.Approximately (draggingX, carDisplayAnchor.localPosition.x);
		}
	}

	public void OnEndDrag (PointerEventData data)
	{
		//Debug.Log (gameObject.GetComponent<RectTransform> ().);
		draggingX = -(centerCar.gameObject.transform.localPosition.x - 305f);
		dragging = true;
	}

	public void OnBeginDrag (PointerEventData data)
	{
		dragging = false;
	}
}
