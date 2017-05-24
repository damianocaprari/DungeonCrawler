using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelController : MonoBehaviour {

    public GameManager gameManager;
    private MouseEventsFSM mouseFSM;
    private Player player;
    private Text nameText;
    private Text classRaceText;
    private Text healthText;
    private Text manaText;
    private Text movementText;
    private Text armorText;
    private Image thumbnail;
    
	void Start () {
        if(gameManager == null) {
            gameManager = FindObjectOfType<GameManager>();
        }
        mouseFSM = FindObjectOfType<MouseEventsFSM>();
        mouseFSM.OnSelectionStateChange += SetSelectedPlayer;

        nameText = transform.Find("Header").Find("Text").GetComponent<Text>();
        classRaceText = transform.Find("Class-Race").GetComponent<Text>();
        healthText = transform.Find("Stats").Find("Text").Find("Health").Find("HealthIcon").Find("HP").GetComponent<Text>();
        manaText = transform.Find("Stats").Find("Text").Find("Mana").Find("ManaIcon").Find("MP").GetComponent<Text>();
        movementText = transform.Find("Stats").Find("Text").Find("Movement").Find("MovementIcon").Find("MS").GetComponent<Text>();
        armorText = transform.Find("Stats").Find("Text").Find("Armor").Find("ArmorIcon").Find("AC").GetComponent<Text>();

        thumbnail = transform.Find("Thumbnail").Find("Image").GetComponent<Image>();

        player = FindObjectOfType<WarriorPlayer>();
    }
	
	void Update () {
        //player = gameManager.GetSelectedPlayer();
        ShowName(player);
        ShowClassRace(player);
        ShowHealth(player);
        ShowMana(player);
        ShowMovement(player);
        ShowArmor(player);
        ShowThumbnail(player);        
    }

    private void SetSelectedPlayer(SelectionStates state, Vector2 position) {
        if(state.Equals(SelectionStates.PLAYER_SELECTED) || state.Equals(SelectionStates.ENEMY_SELECTED)) {
            player = gameManager.GetPlayerByTile(position);
        }
    }

    private void ShowName(Player player)
    {
        nameText.text = player.unitName;
    }
    private void ShowClassRace(Player player)
    {
        classRaceText.text = player.unitRace + " " + player.unitClass;
    }
    private void ShowHealth(Player player)
    {
        string current = player.currentHP.ToString();
        string max = player.maxHP.ToString();
        string str = " HP: " + current + "/" + max;
        healthText.text = str;
    }
    private void ShowMana(Player player)
    {
        if(player.maxSpellPoints == 0)
        {
            manaText.text = " MP: -";
            return;
        }
        string current = player.currentSpellPoints.ToString();
        string max = player.maxSpellPoints.ToString();
        string str = " MP: " + current + "/" + max;
        manaText.text = str;
    }
    private void ShowMovement(Player player)
    {
        string current = player.currentMovementPoints.ToString();
        string max = player.maxMovementPoints.ToString();
        string str = " MS: " + current + "/" + max;
        movementText.text = str;
    }
    private void ShowArmor(Player player)
    {
        string armor = player.armorClass.ToString();
        string str = " AC: " + armor;
        armorText.text = str;
    }
    private void ShowThumbnail(Player player)
    {
        thumbnail.sprite = player.baseSprite;
    }

}
