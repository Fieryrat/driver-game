using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCenterController : Singleton<GameCenterController> {

	private Dictionary<Achievements, string> achDict;

	void Awake ()
	{
		achDict = new Dictionary<Achievements, string> ();

		InfoDoc infoDoc = Tools.ParseDocument ("AchievementsInfo");
		for (int i = 0; i < infoDoc.GetRowCount (); i++) {
			Achievements achName;
			string id;
			infoDoc.TryGetCell<Achievements> (i, "Name", out achName, false);
			if (infoDoc.TryGetCell<string> (i, "ID", out id, false)) {
				achDict [achName] = id;
			}
		}
	}

	public void CheckAchievement (Achievements ach, int info)
	{
		switch (ach) {
			case Achievements.Loser :
			if (info == 0)
				SubmitAchievement (100f, achDict [ach],true);
			break;
			case Achievements.Rich :
			if(info >= 1000)
				SubmitAchievement (0.1f*info, achDict [ach], true);
			break;
			case Achievements.Collector:
			CarsController controller = Singleton<CarsController>.instance;
			if (info == (controller.listCars.Count - 2))
				SubmitAchievement ((100f/(controller.listCars.Count - 2))*info, achDict [ach], true);
			break;
			case Achievements.MusicLover:
			if (info >= 2)
				SubmitAchievement (100f, achDict [ach], true);
			break;
			case Achievements.Inveterate:
			if (info >= 100)
				SubmitAchievement (1f*info, achDict [ach], true);
			break;
			case Achievements.Drift:
			if (info >= 5)
				SubmitAchievement (100f, achDict [ach], true);
			break;
		}
	}

	private void SubmitAchievement(float percent,string id,bool showNotification)
	{
		//IOS
		//GameCenterManager.SubmitAchievement(percent, id, showNotification);
	}

}

public enum Achievements
{
	Loser,
	Rich,
	Collector,
	MusicLover,
	Drift,
	Inveterate
}