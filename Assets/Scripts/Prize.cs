using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Prize : MonoBehaviour {

	[SerializeField] GameObject box;


	private bool boxSelected = false;
	private Box selectedBox = null;
	[SerializeField] Image white = null;

	//After open box
	[SerializeField] GameObject rays = null;
	[SerializeField] Image car = null;
	[SerializeField] Text prizeName = null;
	[SerializeField] GameObject toContinue = null;
	[SerializeField] GameObject evetHook = null;
	
	public void OpenBox () {
		if (!boxSelected) {
			boxSelected = true;
			if (Singleton<GameController>.instance.coin >= 100 && Singleton<CarsController>.instance.prizeCar != null ) {
				Singleton<CarsController>.instance.UnlockPrizeCar ();
				box.transform.SetAsLastSibling ();
				selectedBox = box.GetComponent<Box> ();
				//LeanTween.scale( boxesArray [i], new Vector3(2f, 2f, 2f), 1.2f).setEase(LeanTweenType.easeOutBounce).setOnComplete (() => {selectedBox.Open(); FadeIn();});
				selectedBox.Open ();
				FadeIn ();
			} else {
				gameObject.SetActive (false);
			}
		}
	}
	
	void OnEnable () {
		boxSelected = false;
		box.GetComponent<RectTransform>().anchoredPosition = new Vector2(25f,0f);
		//box.transform.localPosition = Vector3.one;
		box.SetActive (true);
		rays.SetActive (false);
		toContinue.SetActive (false);
		car.gameObject.SetActive (false);
		prizeName.gameObject.SetActive (false);
		evetHook.SetActive(false);
	}
	
	public void FadeIn()
	{
		white.gameObject.SetActive (true);
		white.gameObject.transform.SetAsLastSibling();
		LeanTween.value (gameObject, setSpriteAlpha, 0f, 1f, 1f);
	}
   
	//После того как выбрали коробку и белый свет ушел.
    public void FadeOut()
    {
		toContinue.SetActive (true);
		//Лучи
		rays.SetActive (true);
		LeanTween.rotateAround( rays, Vector3.forward, 360f, 10f).setLoopClamp();
		//Машина
		car.sprite = Singleton<CarsController>.instance.prizeCar.carSprite;
		car.gameObject.SetActive (true);
		//Имя машины
		prizeName.text = Singleton<CarsController>.instance.prizeCar.carName;
		prizeName.gameObject.SetActive (true);

		//Уловительнажатий.
		evetHook.transform.SetAsLastSibling();
		evetHook.SetActive(true);

		//Белена.
		LeanTween.value( gameObject, setSpriteAlpha, 1f, 0f, 0.15f ).setOnComplete (() => white.gameObject.SetActive (false));
    }
   
    private void setSpriteAlpha( float val )
    {
        white.color = new Color(1f,1f,1f,val);
    }
 
}
