using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType {
    PLAYER_ALLIED,
    PLAYER_ENEMY,
    TRAP_REVEALED,
    DOOR_CLOSED,
    CHEST_CLOSED,
    GROUND,
    OTHER
}

public abstract class InputState : MonoBehaviour {
    protected GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public abstract InputState HandleInput(int mouseButtonId, Tile tilePressed);

    public virtual void Enter() { }

    protected EntityType ClickedEntity(Tile tilePressed)
    {
        if(tilePressed.type == TileType.COLUMN || tilePressed.type == TileType.WALL || tilePressed.type == TileType.TRANSPARENT)
        {
            return EntityType.OTHER;
        }
        Player player = gameManager.GetPlayerByTile(tilePressed);
        if (player != null)
        {
            if (player.isPlayerControlled)
            {
                return EntityType.PLAYER_ALLIED;
            } else
            {
                return EntityType.PLAYER_ENEMY;
            }
        }
        if(tilePressed.type == TileType.DOOR)
        {
            if (tilePressed.isDoorClosed)
            {
                return EntityType.DOOR_CLOSED;
            } else
            {
                return EntityType.GROUND;
            }
        }
        if(tilePressed.type == TileType.CHEST)
        {
            if (tilePressed.isChestClosed)
            {
                return EntityType.CHEST_CLOSED;
            } else
            {
                return EntityType.OTHER;
            }
        }
        if(tilePressed.type == TileType.TRAP)
        {
            if (tilePressed.isTrapRevealed)
            {
                return EntityType.TRAP_REVEALED;
            } else
            {
                return EntityType.GROUND;
            }
        }
        return EntityType.OTHER;
    }
}
