using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    public BattleMenu battleMenu;

    private void Start()
    {
        transform.position = getPosition(battleMenu.isTargetAlly, battleMenu.currentTarget);
    }

    private void LateUpdate()
    {
        transform.position = getPosition(battleMenu.isTargetAlly, battleMenu.currentTarget);
    }

    private Vector2 getPosition(bool isAlly, int index)
    {
        if (isAlly)
        {
            return new Vector2(-2 + index * -2, -2);
        }
        else
        {
            return new Vector2(2 + index * 2, -2);
        }
    }
}
