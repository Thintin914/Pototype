﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    public bool isDescription;
    public Sprite[] skillSprites;
    private SpriteRenderer sr;
    public BattleMenu battleMenu;
    public int scroller = 0, descriptionID;
    public GameObject description1, description2;
    private SkillMenu skillMenu;
    private List<Skill> skills;
    private Character characterStats;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        skills = battleMenu.database.allyDetails[battleMenu.database.selector].GetComponent<Character>().skills;
        sr.sortingOrder = (int)transform.position.z * -1;
        if (isDescription == false)
        {
            battleMenu.instructionHolder.text = "[W], [Up] and [S], [Down] to scroll, [Z] to comfirm, [X] to cancel";
            description1 = Instantiate(gameObject, transform.position, Quaternion.identity);
            description1.GetComponent<SkillMenu>().isDescription = true;
            description1.GetComponent<SkillMenu>().descriptionID = 0;
            description1.name = "Page1";

            description2 = Instantiate(gameObject, transform.position, Quaternion.identity);
            description2.GetComponent<SkillMenu>().isDescription = true;
            description2.GetComponent<SkillMenu>().descriptionID = 1;
            description2.name = "Page2";

            sr.sprite = skillSprites[10];
            characterStats = battleMenu.database.allyDetails[battleMenu.database.selector].GetComponent<Character>();
        }
        else
        {
            transform.SetParent(GameObject.Find("SkillMenu(Clone)").transform);
            skillMenu = transform.parent.GetComponent<SkillMenu>();
            if (descriptionID + skillMenu.scroller < skills.Count)
            {
                sr.sprite = skillSprites[skills[skillMenu.scroller].ID + descriptionID];
            }
            else
            {
                Destroy(gameObject);
            }

            if (descriptionID == 0)
            {
                transform.position = (Vector2)transform.position + new Vector2(0, 0.75f);
            }
            else
            {
                transform.position = (Vector2)transform.position + new Vector2(0, -0.75f);
                sr.color = new Color32(170, 70, 200, 255);
            }
            transform.position += new Vector3(0, 0, -1);

        }
    }

    private void Update()
    {
        if (isDescription == true)
        {
            int temp = skillMenu.scroller + descriptionID;
            if (temp >= skills.Count)
            {
                sr.sprite = null;
            }
            else
            {
                sr.sprite = skillSprites[skills[temp].ID];
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                battleMenu.instructionHolder.text = "[W], [Up] and [S], [Down] to scroll, [Z] to comfirm";
                battleMenu.isSelectedOption = false;
                Destroy(gameObject);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (scroller + 2 <= skills.Count)
                {
                    scroller++;
                }
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (scroller - 1 >= 0)
                {
                    scroller--;
                }
            }
            if (Input.GetKeyDown(KeyCode.Z) && skills.Count != 0)
            {
                if (characterStats.currentMP >= skills[scroller].MPCost)
                {
                    characterStats.currentMP -= skills[scroller].MPCost;
                    battleMenu.currentItem = skills[scroller].ID;
                    battleMenu.isSelectedItem = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
