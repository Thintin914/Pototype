﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public Sprite[] battleOptions;
    public GameObject targetIcon, skillMenu;
    private char targetRange = 'e';
    private GameObject targetIconHolder, skillMenuHolder;

    public Database database;
    private SpriteRenderer sr;
    public bool isShowing = false, isSelectedOption = false, isTargetAlly = false, isSelectedTarget = false, isSelectedItem = false, hasTargetIconCreated = false, hasTargetRangeSet = false;
    public int currentOption = 0, currentTarget = 0, currentItem = 0;

    private void Awake()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void Show()
    {
        sr.enabled = true;
        isShowing = true;
        isSelectedOption = false;
        isTargetAlly = false;
        isSelectedTarget = false;
        isSelectedItem = false;
        hasTargetIconCreated = false;
        hasTargetRangeSet = false;
        sr.sprite = battleOptions[0];
        currentOption = 0;
        currentTarget = 0;
        currentItem = 0;
    }
    public void Hide()
    {
        sr.enabled = false;
        isShowing = false;
    }

    private void Update()
    {
        if (isShowing == true)
        {
            if (isSelectedOption == false)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (currentOption - 1 > 1)
                    {
                        currentOption--;
                        sr.sprite = battleOptions[currentOption];
                    }
                    else
                    {
                        currentOption = 1;
                        sr.sprite = battleOptions[currentOption];
                    }
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (currentOption + 1 < 4)
                    {
                        currentOption++;
                        sr.sprite = battleOptions[currentOption];
                    }
                    else
                    {
                        currentOption = 3;
                        sr.sprite = battleOptions[currentOption];
                    }
                }
                if (Input.GetKeyDown(KeyCode.Z) && currentOption != 0)
                {
                    switch (currentOption)
                    {
                        case 1:
                            targetRange = 'e';
                            isTargetAlly = false;
                            CreateTargetIcon();
                            break;
                        case 2:
                            skillMenuHolder = Instantiate(skillMenu);
                            skillMenuHolder.GetComponent<SkillMenu>().battleMenu = this;
                            skillMenuHolder.GetComponent<SkillMenu>().isDescription = false;
                            break;
                        case 3:
                            break;
                    }
                    isSelectedOption = true;
                }
            }
            else
            {
                switch (currentOption)
                {
                    case 1://Attack
                        selectTarget();
                        break;
                    case 2://Skill
                        if(isSelectedItem == true)
                        {
                            CreateTargetIcon();
                            getSkillTargetRange();
                            selectTarget();
                        }
                        break;
                }
            }
        }
    }

    private void CreateTargetIcon()
    {
        if (hasTargetIconCreated == false)
        {
            hasTargetIconCreated = true;
            targetIconHolder = Instantiate(targetIcon);
            targetIconHolder.GetComponent<TargetSelection>().battleMenu = this;
        }
    }

    private void selectTarget()
    {
        if (isSelectedTarget == false)
        {
            if (targetRange != 's')
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (isTargetAlly == false)
                    {
                        if (currentTarget - 1 >= 0)
                        {
                            currentTarget--;
                        }
                        else if (targetRange != 'e')
                        {
                            currentTarget = 0;
                            isTargetAlly = true;
                        }
                    }
                    else
                    {
                        if (currentTarget + 1 < database.allyDetails.Count)
                        {
                            currentTarget++;
                        }
                        else
                        {
                            currentTarget = database.allyDetails.Count - 1;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (isTargetAlly == false)
                    {
                        if (currentTarget + 1 < database.enemyDetails.Count)
                        {
                            currentTarget++;
                        }
                        else
                        {
                            currentTarget = database.enemyDetails.Count - 1;
                        }
                    }
                    else
                    {
                        if (currentTarget - 1 >= 0)
                        {
                            currentTarget--;
                        }
                        else if (targetRange != 'a')
                        {
                            currentTarget = 0;
                            isTargetAlly = false;
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isSelectedTarget = true;
            }
            if (Input.GetKeyDown(KeyCode.X) && isSelectedTarget == false)
            {
                Destroy(targetIconHolder);
                isSelectedOption = false;
                isSelectedItem = false;
                hasTargetIconCreated = false;
                hasTargetRangeSet = false;
                targetRange = 'e';
            }
        }
        else
        {
            Hide();
            Destroy(targetIconHolder);
            database.isAllySelected = isTargetAlly;
            database.selectedIndex = currentTarget;
            database.selectedState = currentOption - 1;
            database.selectedItem = currentItem;
            database.isSelectedOption = true;
        }
    }

    private void getSkillTargetRange()
    {
        if (hasTargetRangeSet == false)
        {
            hasTargetRangeSet = true;
            switch (currentItem)
            {
                case 0:
                    targetRange = 's';
                    break;
                case 1:
                    targetRange = 'e';
                    break;
                case 2:
                    targetRange = 'e';
                    break;
                case 3:
                    targetRange = 'e';
                    break;
                case 4:
                    targetRange = 'a';
                    break;
                case 5:
                    targetRange = 'e';
                    break;
                case 6:
                    targetRange = 'a';
                    break;
                case 7:
                    targetRange = 'a';
                    break;
                case 8:
                    targetRange = 'u';
                    break;
                case 9:
                    targetRange = 'e';
                    break;
            }
            switch (targetRange)
            {
                case 's':
                    isTargetAlly = true;
                    currentTarget = database.selector;
                    break;
                case 'u':
                case 'e':
                    isTargetAlly = false;
                    currentTarget = 0;
                    break;
                case 'a':
                    isTargetAlly = true;
                    currentTarget = 0;
                    break;
            }
        }
    }
}