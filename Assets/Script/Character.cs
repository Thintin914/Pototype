﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHP, currentHP, maxMP, currentMP, defense, dodgeRate, speed, attackDamage, ID;
    public int extraDefense, extraDodgeRate, extraSpeed, extraAttackDamage;
    public enum Element {none, fire, water, wind, earth, electricity, ice, rain, stone, wildfire, chaos};
    public Element element;
    public bool isAlly, isDead = false;
    public SceneCharacter sceneCharacter;
    public List<Skill> skills;
    public GameObject elementPrefab;
    public List<GameObject> effects;
    public GameObject textPrefab;

    public void SetCharacter(int maxHP, int maxMP, int defense, int dodgeRate, int speed, int attackDamage, Element element, int ID)
    {
        this.maxHP = maxHP;
        currentHP = maxHP;
        this.maxMP = maxMP;
        currentMP = 0;
        this.defense = defense;
        this.dodgeRate = dodgeRate;
        this.speed = speed;
        this.attackDamage = attackDamage;
        this.element = element;
        this.ID = ID;
    }

    public void AddSkill(Skill skill)
    {
        skills.Add(skill);
    }

    public void AddEffect(int round, Element element)
    {
        if (element != Element.none)
        {
            GameObject temp = Instantiate(elementPrefab);
            temp.GetComponent<ElementEffect>().setValue(round, element, this);
            effects.Add(temp);
        }
    }
}
