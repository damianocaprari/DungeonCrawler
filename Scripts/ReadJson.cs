using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

/*
 * SCRIPT NON UTILIZZATO, DA ELIMINARE PROBABILMENTE A FINE PROGETTO
 */

public class ReadJson : MonoBehaviour {

	private string jsonString;
	private JsonData itemData;

	// Use this for initialization
	void Start () {
		jsonString = File.ReadAllText(Application.dataPath + "/Resources/Items.json");
		itemData = JsonMapper.ToObject(jsonString);

		Debug.Log(itemData["Weapons"][1]["name"]);
		Debug.Log(GetItem("Light Rifle", "Weapons"));
	}
	
	JsonData GetItem(string name, string type) {
		for(int i = 0; i < itemData[type].Count; i++) {
			if(itemData[type][i]["name"].ToString() == name || itemData[type][i]["slug"].ToString() == name) {
				return itemData[type][i];
			}
		}

		return null;
	}
}
