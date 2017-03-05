using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

/*
 * SCRIPT NON UTILIZZATO, DA ELIMINARE PROBABILMENTE A FINE PROGETTO
 */
public class WriteJson : MonoBehaviour {

	public TempPlayer player = new TempPlayer(0, "default", 10, false, new int[] { 7, 4, 8, 21, 12, 123 });
	JsonData playerJson;
    
	void Start () {
		playerJson = JsonMapper.ToJson(player);
		File.WriteAllText(Application.dataPath + "/Player.json", playerJson.ToString());

		
	}
	
	void Update () {
	
	}
}

public class TempPlayer {

	public int id;
	public string name;
	public int health;
	public bool aggressive;
	public int[] stats;

	public TempPlayer(int id, string name, int health, bool aggressive, int[] stats) {
		this.id = id;
		this.name = name;
		this.health = health;
		this.aggressive = aggressive;
		this.stats = stats;
	}
	
}
