using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Tools : MonoBehaviour {

	public static InfoDoc ParseDocument(string sheetName)
	{
		string str = LoadAsText(sheetName);
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		List<List<string>> data = new List<List<string>>();
		Dictionary<string, int> columnDict = new Dictionary<string, int>();
		List<string> item = new List<string>();
		string str2 = string.Empty;
		bool flag = false;
		for (int i = 0; i < str.Length; i++)
		{
			if ((str[i] == ',') && !flag)
			{
				item.Add(str2);
				str2 = string.Empty;
			}
			else if ((str[i] == '\r') && !flag)
			{
				item.Add(str2);
				data.Add(item);
				str2 = string.Empty;
				item = new List<string>();
				//i++;
			}
			//Если кавычка
			else if (str[i] == '"')
			{
				//Врубаем флаг если не был включен
				if (!flag)
				{
					flag = true;
				}
				//Иначе Если Следующий символ не последний в документе и следующий символ является кавычкой
				else if (((i + 1) < str.Length) && (str[i + 1] == '"'))
				{
					//Добавляем кавычку и переходим к следующему.
					str2 = str2 + '"';
					i++;
				}
				//Если флаг включен, то выклчюаем его
				else
				{
					flag = false;
				}
			}
			else
			{
				str2 = str2 + str[i];
			}
		}
		item.Add(str2);
		data.Add(item);
		List<string> list3 = data[0];
		for (int j = 0; j < list3.Count; j++)
		{
			//Debug.Log(string.Format("List element #{0} in shetname {2} = {1}",j,list3[j],sheetName));
			columnDict[list3[j]] = j;
		}
		data.RemoveAt(0);
		return new InfoDoc(data, columnDict);
	}

	public static string LoadAsText(string sheetName)
	{
		string text = string.Empty;
		string path = string.Format("{0}{1}{2}.txt", GetDocumentPath(), "gamedata/", sheetName);
		if (File.Exists(path))
		{
			Stream stream = File.Open(path, FileMode.Open);
			StreamReader reader = new StreamReader(stream);
			text = reader.ReadToEnd();
			reader.Close();
			stream.Close();
			return text;
		}
		TextAsset asset = Resources.Load<TextAsset>("Documents/" + sheetName);
		if (asset != null)
		{
			text = asset.text;
		}
		return text;
	}

	public static string GetDocumentPath()
	{
		string persistentDataPath = Application.persistentDataPath;
		if (Application.platform == RuntimePlatform.Android)
		{
			return (Application.persistentDataPath.Replace(".apk", "/Documents/") + "/");
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return (Application.persistentDataPath + "/");
		}
		if ((!Application.isEditor && (Application.platform != RuntimePlatform.OSXPlayer)) && (Application.platform != RuntimePlatform.WindowsPlayer))
		{
			return persistentDataPath;
		}
		return string.Empty;
	}
}
