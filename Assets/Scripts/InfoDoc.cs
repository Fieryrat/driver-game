using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InfoDoc {

	protected Dictionary<string, int> allColumnIDs;
	protected List<List<string>> allRowData;

	public InfoDoc(List<List<string>> data, Dictionary<string, int> columnDict)
	{
		this.allRowData = data;
		this.allColumnIDs = columnDict;
	}

	public string GetCellString(int rowNum, string columnName)
	{
		string str = string.Empty;
		if (((rowNum >= 0) && (rowNum < this.allRowData.Count)) && this.allColumnIDs.ContainsKey(columnName))
		{
			List<string> list = this.allRowData[rowNum];
			int num = this.allColumnIDs[columnName];
			str = list[num];
		}
		return str;
	}

	public List<string> GetRow(int row)
	{
		if (row < this.allRowData.Count)
		{
			return this.allRowData[row];
		}
		return null;
	}

	public int GetRowCount()
	{
		return this.allRowData.Count;
	}
		
	public bool TryGetCell<T>(int rowNum, string columnName, out T cell, bool showWarning = false)
	{
		cell = default(T);
		if ((rowNum < 0) || (rowNum >= this.allRowData.Count))
		{
			if (showWarning)
			{
				Debug.LogError(rowNum + " row not found.");
			}
			return false;
		}
		if (!this.allColumnIDs.ContainsKey(columnName))
		{
			if (showWarning)
			{
				Debug.LogError(columnName + " column not found.");
			}
			return false;
		}
		List<string> list = this.allRowData[rowNum];
		int num = this.allColumnIDs[columnName];
		try
		{
			if (string.IsNullOrEmpty(list[num]))
			{
				//Debug.Log("List[null] empty!");
				return false;
			}
			if (typeof(T).IsEnum)
			{
				cell = (T) Enum.Parse(typeof(T), list[num]);
			}
			else
			{
				cell = (T) Convert.ChangeType(list[num], typeof(T));
			}
		}
		catch (Exception exception)
		{
			if (showWarning)
			{
				Debug.LogError("ChangeType failed: " + exception.ToString() + ": convert " + list[num] + " to " + typeof(T).ToString());
			}
			return false;
		}
		return true;
	}




}
