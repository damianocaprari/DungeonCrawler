using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum PlayerFaction {
    PlayerControlled,
    // AIAlly,
    AIEnemy
}

public class Player : MonoBehaviour {

    //engine variables		##########################
    public Vector2 position = Vector2.zero;
    public List<Tile> currentPath;
    public TileMap map;
    public GameManager gameManager;

    //animation variables
    public Animator animator;
    public float moveSpeed;
    public float animMoveX;
    public float animMoveY;
    //	public float animLastMoveX;
    //	public float animLastMoveY;
    public bool isMoving;
    public bool isAttacking;

    public Sprite baseSprite;

    Image healthBar;
    //end animation variables

    //end engine variables	##########################
    //
    //ingame attributes		##########################
    public string unitName;
    public string unitClass;
    public string unitRace;
    public PlayerFaction faction;

    public bool isPlayerControlled {
        get {
            return faction == PlayerFaction.PlayerControlled;
        }
    }
    public int level = 1;

    public int maxHP = 8;
    public int currentHP;

    public List<AttackDieTypes> attackDices;
    public int armorClass;
    public int initiative;

    public int maxActionPoints;
    public int currentActionPoints;

    public int maxMovementPoints;
    public int currentMovementPoints;
    public int attackRange;

    public int spellSlots;
    public int maxSpellPoints;
    public int currentSpellPoints;

    public int bagSlots;
    public bool hasSpecialActions = false;
    //end ingame attributes	##########################


    protected void Start() {
        map = FindObjectOfType<TileMap>();
        gameManager = FindObjectOfType<GameManager>();
        animMoveX = 0;
        animMoveY = -1;
        isMoving = false;
        animator = GetComponentInChildren<Animator>();
        healthBar = transform.Find("Canvas").Find("HealthBG").Find("Health").GetComponent<Image>();
        baseSprite = transform.Find("Visuals").GetComponent<SpriteRenderer>().sprite;
    }

    protected void Update() {
        MoveAlongPath();
        UpdateAnimator();
    }

    void UpdateAnimator() {
        animator.SetFloat("MoveX", animMoveX);
        animator.SetFloat("MoveY", animMoveY);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsAttacking", isAttacking);
        //Debug.Log("MoveX: " + animMoveX + "\t\t|\tMoveY: " + animMoveY + "\nLastMoveX: " + animLastMoveX + "\t|\tLastMoveY: " + animLastMoveY + "\nIsMoving: " + isMoving);
    }

    private void DrawPathLine() {
        if(currentPath != null) {
            int currNode = 0;
            while(currNode < currentPath.Count - 1) {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[currNode].nodePosition);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currNode + 1].nodePosition);
                Debug.DrawLine(start, end, Color.red);
                currNode++;
            }
        }
    }

    public void MoveAlongPath() {
        float onTargetTolerance = 0.1f;
        if(currentPath != null) {
            if(currentPath.Count >= 2) {
                //Debug.Log("Tile mia: " + transform.position + "| Tile da raggiungere: " + map.TileCoordToWorldCoord(currentPath[1].nodePosition));
                if(currentPath[1].nodePosition.y < currentPath[0].nodePosition.y) { //MOVE UP
                    if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord(currentPath[1].nodePosition)) > onTargetTolerance) {
                        //i still need to reach the new node
                        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                        animMoveY = 1;
                        animMoveX = 0;
                        isMoving = true;
                    }
                    else {
                        transform.position = map.TileCoordToWorldCoord(currentPath[1].nodePosition);
                        position = currentPath[1].nodePosition;
                        currentPath.RemoveAt(0);
                        currentMovementPoints--;
                        if(currentMovementPoints == 0) {
                            currentPath.Clear();
                            currentMovementPoints = maxMovementPoints;
                        }
                        if(currentPath.Count < 2)
                            isMoving = false;
                    }
                }
                else if(currentPath[1].nodePosition.y > currentPath[0].nodePosition.y) {    //MOVE DOWN
                    if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord(currentPath[1].nodePosition)) > onTargetTolerance) {
                        //i still need to reach the new node
                        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                        animMoveY = -1;
                        animMoveX = 0;
                        isMoving = true;
                    }
                    else {
                        transform.position = map.TileCoordToWorldCoord(currentPath[1].nodePosition);
                        position = currentPath[1].nodePosition;
                        currentPath.RemoveAt(0);
                        currentMovementPoints--;
                        if(currentMovementPoints == 0) {
                            currentPath.Clear();
                            currentMovementPoints = maxMovementPoints;
                        }
                        if(currentPath.Count < 2)
                            isMoving = false;
                    }
                }
                else if(currentPath[1].nodePosition.x < currentPath[0].nodePosition.x) {    //MOVE LEFT
                    if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord(currentPath[1].nodePosition)) > onTargetTolerance) {
                        //i still need to reach the new node
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                        animMoveY = 0;
                        animMoveX = -1;
                        isMoving = true;
                    }
                    else {
                        transform.position = map.TileCoordToWorldCoord(currentPath[1].nodePosition);
                        position = currentPath[1].nodePosition;
                        currentPath.RemoveAt(0);
                        currentMovementPoints--;
                        if(currentMovementPoints == 0) {
                            currentPath.Clear();
                            currentMovementPoints = maxMovementPoints;
                        }
                        if(currentPath.Count < 2)
                            isMoving = false;
                    }
                }
                else if(currentPath[1].nodePosition.x > currentPath[0].nodePosition.x) {    //MOVE RIGHT
                    if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord(currentPath[1].nodePosition)) > onTargetTolerance) {
                        //i still need to reach the new node
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                        animMoveY = 0;
                        animMoveX = 1;
                        isMoving = true;
                    }
                    else {
                        transform.position = map.TileCoordToWorldCoord(currentPath[1].nodePosition);
                        position = currentPath[1].nodePosition;
                        currentPath.RemoveAt(0);
                        currentMovementPoints--;
                        if(currentMovementPoints == 0) {
                            currentPath.Clear();
                            currentMovementPoints = maxMovementPoints;
                        }
                        if(currentPath.Count < 2)
                            isMoving = false;
                    }
                }
            }
            else {
                currentPath.Clear();
            }
        }
        DrawPathLine();
    }

    public virtual void DealDamage(Player target) {
        int damage = 0;
        foreach(AttackDieTypes a in attackDices) {
            damage += AttackDie.RollDice(a);
        }
        target.GetDamaged(damage);
        currentActionPoints--;
    }


    public void GetDamaged(int amount) {
        if(amount > armorClass) {
            currentHP -= (amount - armorClass);
        }
        if(currentHP <= 0) {
            Death();
        }
        healthBar.fillAmount = (float)currentHP / (float)maxHP;
    }

    public void GetHealed(int amount) {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        healthBar.fillAmount = (float)currentHP / (float)maxHP;
    }

    void Death() {
        animator.SetTrigger("IsDead");
        gameManager.RemovePlayerFromList(this);
        Destroy(gameObject, 0.9f);
    }

    public void SetMarkerColor(Color color) {
        transform.Find("Canvas").Find("Marker").GetComponent<Image>().color = color;
    }

    virtual public void FirePlayerSelectionEvent() { }
    virtual public void FirePlayerActionEvent() { }

}
