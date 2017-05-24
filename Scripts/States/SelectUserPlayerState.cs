//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SelectUserPlayerState : InputState
//{    
//    public override InputState HandleInput(int mouseButtonId, Tile tilePressed)
//    {
//        EntityType clickedEntityType = ClickedEntity(tilePressed);
//        switch (mouseButtonId)
//        {
//            case 0: //left mouse button. perform selection
//                switch (clickedEntityType)
//                {
//                    case EntityType.CHEST_CLOSED:
//                        return new SelectChestState();                       
//                    case EntityType.PLAYER_ALLIED:
//                        return new SelectUserPlayerState(); 
//                    case EntityType.PLAYER_ENEMY:
//                        return new SelectEnemyState(); 
//                    case EntityType.TRAP_REVEALED:
//                        return new SelectTrapState(); 
//                    default:
//                        return new SelectNothingState(); 
//                }
//            case 1: //right mouse button. perform action
//                switch (clickedEntityType)
//                {
//                    case EntityType.CHEST_CLOSED:
//                        OpenChestAction(tilePressed); return this;
//                    case EntityType.DOOR_CLOSED:
//                        OpenDoorAction(tilePressed); return this;
//                    case EntityType.GROUND:
//                        MovePlayerAction(tilePressed); return this;
//                    case EntityType.PLAYER_ALLIED:
//                        HealPlayerAction(tilePressed); return this;
//                    case EntityType.PLAYER_ENEMY:
//                        AttackEnemyAction(tilePressed); return this;
//                    case EntityType.TRAP_REVEALED:
//                        DefuseTrapAction(tilePressed); return this;
//                    case EntityType.OTHER:
//                        return new SelectNothingState();
//                    default:
//                        return new SelectNothingState();
//                }
//        }

//    }

//    void OpenChestAction(Tile tilePressed)
//    {
//        Vector2 playerPosition = gameManager.GetSelectedPlayer().position;
//        Vector2 chestPosition = tilePressed.nodePosition;
//        float distance = Mathf.Abs(playerPosition.x - chestPosition.x) + Mathf.Abs(playerPosition.y - chestPosition.y);
//        if(distance <= 1)
//        {
//            tilePressed.OpenChest();
//        }
//    }

//    void OpenDoorAction(Tile tilePressed)
//    {        
//        Vector2 playerPosition = gameManager.GetSelectedPlayer().position;
//        Vector2 chestPosition = tilePressed.nodePosition;
//        float distance = Mathf.Abs(playerPosition.x - chestPosition.x) + Mathf.Abs(playerPosition.y - chestPosition.y);
//        if (distance <= 1)
//        {
//            tilePressed.OpenDoor();
//        }
//    }
//}