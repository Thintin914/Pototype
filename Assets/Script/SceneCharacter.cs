using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCharacter : MonoBehaviour
{
    public bool isBarCharacter;
    public Character characterStats;
    public Database database;
    public Animator animator;
    public SceneCharacter sceneCharacter, barCharacter;
    public SpriteRenderer myRenderer;
    public int repeatRate = 0;
    public float progress = 0;

    private BattleMenu battleMenu;
    private static int speedBarLength = 16;
    private Shader shaderGUIText, shaderSpriteDefault;
    private TMPro.TextMeshProUGUI HPIndicatorHolder;

    private void Start()
    {
        if (isBarCharacter == false)
        {
            GameObject cloner1 = Instantiate(database.characterSprites[characterStats.ID], transform.position, Quaternion.identity);
            cloner1.transform.SetParent(transform);
            animator = cloner1.GetComponent<Animator>();
            myRenderer = cloner1.GetComponent<SpriteRenderer>();

            GameObject cloner2 = Instantiate(gameObject);
            barCharacter = cloner2.GetComponent<SceneCharacter>();
            barCharacter.isBarCharacter = true;
            barCharacter.name = "barIcon";
            barCharacter.transform.SetParent(transform);

            HPIndicatorHolder = Instantiate(characterStats.textPrefab).GetComponent<TMPro.TextMeshProUGUI>();
            HPIndicatorHolder.transform.position = transform.position;
            HPIndicatorHolder.transform.position += new Vector3(1, -1.6f, 0);
            HPIndicatorHolder.transform.SetParent(GameObject.Find("Canvas").transform);
            if (characterStats.isAlly == true)
            {
                HPIndicatorHolder.text = "HP" + characterStats.currentHP + "\nMP" + characterStats.currentMP;
            }
            else
            {
                HPIndicatorHolder.text = "HP" + characterStats.currentHP;
            }
            HPIndicatorHolder.fontSize = 0.2f;
            if (tag == "Enemy")
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector2(0.4f, 0.4f);
            animator = transform.GetChild(0).GetComponent<Animator>();
            myRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            battleMenu = GameObject.Find("BattleMenu").GetComponent<BattleMenu>();
            sceneCharacter = transform.parent.GetComponent<SceneCharacter>();
        }
        shaderGUIText = Shader.Find("GUI/Text Shader");
        shaderSpriteDefault = Shader.Find("Sprites/Default");
    }

    private void FixedUpdate()
    {
        if (database.isHandling == false)
        {
            if (isBarCharacter == true)
            {
                if (characterStats.isDead == false)
                {
                    if (progress <= 0)
                    {
                        transform.position = new Vector2(-8, 4);
                        progress += 0.1f;
                    }
                    else if (progress >= speedBarLength)
                    {
                        if (database.isHandling == false)
                        {
                            database.isHandling = true;
                            if (characterStats.isAlly == true)
                            {
                                StartCoroutine("WaitOption");
                            }
                            else
                            {
                                performAttackPattern(characterStats.ID, getAttackID(characterStats.ID, repeatRate), 0, true);
                                StartCoroutine("playAnimation");
                            }
                        }
                    }
                    else
                    {
                            if (characterStats.speed + characterStats.extraSpeed > 0)
                            {
                                progress += (characterStats.speed + characterStats.extraSpeed) * Time.deltaTime;
                            }
                            else
                            {
                                progress += 1 * Time.deltaTime;
                            }
                            transform.position = new Vector2(-8 + progress, 4);
                    }
                }
                else
                {
                    transform.position = new Vector2(-8, 4);
                    progress = 0;
                    myRenderer.material.shader = shaderGUIText;
                    myRenderer.color = Color.grey;
                }
            }
            else
            {
                if (characterStats.currentHP <= 0 && characterStats.isDead == false)
                {
                    HPIndicatorHolder.text = "Dead";
                    if (characterStats.isAlly == true)
                    {
                        database.allyDetails.Remove(characterStats.gameObject);
                        database.allyDetails.Add(characterStats.gameObject);
                        characterStats.isDead = true;
                        myRenderer.material.shader = shaderGUIText;
                        myRenderer.color = Color.grey;

                        int deathNumber = 0;
                        for (int i = 0; i < database.allyDetails.Count; i++)
                        {
                            Character tempCharacter = database.allyDetails[i].GetComponent<Character>();
                            if (tempCharacter.isDead == true)
                            {
                                deathNumber++;
                            }
                            tempCharacter.sceneCharacter.transform.position = new Vector2(-2 + i * -2, -2);
                            TMPro.TextMeshProUGUI tempHPIndicator = tempCharacter.sceneCharacter.GetComponent<SceneCharacter>().HPIndicatorHolder;
                            tempHPIndicator.transform.position = tempCharacter.sceneCharacter.transform.position;
                            tempHPIndicator.transform.position += new Vector3(1, -1.6f, 0);
                        }
                        if (deathNumber == database.allyDetails.Count)
                        {
                            Debug.Log("All Allies Died.");
                        }
                    }
                    else
                    {
                        database.enemyDetails.Remove(characterStats.gameObject);
                        database.enemyDetails.Add(characterStats.gameObject);
                        characterStats.isDead = true;
                        myRenderer.material.shader = shaderGUIText;
                        myRenderer.color = Color.grey;

                        int deathNumber = 0;
                        for (int i = 0; i < database.enemyDetails.Count; i++)
                        {
                            Character tempCharacter = database.enemyDetails[i].GetComponent<Character>();
                            if (tempCharacter.isDead == true)
                            {
                                deathNumber++;
                            }
                            tempCharacter.sceneCharacter.transform.position = new Vector2(2 + i * 2, -2);
                            TMPro.TextMeshProUGUI tempHPIndicator = tempCharacter.sceneCharacter.GetComponent<SceneCharacter>().HPIndicatorHolder;
                            tempHPIndicator.transform.position = tempCharacter.sceneCharacter.transform.position;
                            tempHPIndicator.transform.position += new Vector3(1, -1.6f, 0);
                        }
                        if (deathNumber == database.enemyDetails.Count)
                        {
                            Debug.Log("All Enemies Died.");
                        }
                    }

                }
                else
                {
                    if (characterStats.isAlly == true)
                    {
                        HPIndicatorHolder.text = "HP" + characterStats.currentHP + "\nMP" + characterStats.currentMP;
                    }
                    else
                    {
                        HPIndicatorHolder.text = "HP" + characterStats.currentHP;
                    }
                    if (characterStats.currentHP <= 0)
                    {
                        HPIndicatorHolder.text = "Dead";
                    }
                }
            }
        }
    }

    public void isHit()
    {
        StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        myRenderer.material.shader = shaderGUIText;
        myRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        if (characterStats.isDead == true)
        {
            myRenderer.material.shader = shaderGUIText;
            myRenderer.color = Color.grey;
        }
        else
        {
            myRenderer.material.shader = shaderSpriteDefault;
            myRenderer.color = Color.white;
        }
    }

    IEnumerator playAnimation()
    {
        sceneCharacter.animator.SetBool("isAttack", true);
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).speed);
        repeatRate++;
        sceneCharacter.animator.SetBool("isAttack", false);
        animator.SetBool("isAttack", false);
        database.isHandling = false;
        progress = 0;
    }

    // For Enemy Only
    private int getAttackID(int ID, int repeatRate)
    {
        int maxSkillPattern = 0;
        switch (ID)
        {
            case 0:
                maxSkillPattern = 1;
                break;
            default:
                maxSkillPattern = -1;
                break;
        }
        if (maxSkillPattern == -1)
        {
            return -1;
        }
        return repeatRate % maxSkillPattern;
    }

    private Character getTarget(int index, bool isAlly)
    {
        if (isAlly)
        {
            return database.allyDetails[index].GetComponent<Character>();
        }
        else
        {
            return database.enemyDetails[index].GetComponent<Character>();
        }
    }

    // For Enemy Only, AttackID Sets To -1 = Basic Attack
    private void performAttackPattern(int ID, int attackID, int index, bool isAlly)
    {
        Character target = getTarget(index, isAlly);
        int finalAttackDamage = (characterStats.attackDamage + characterStats.extraAttackDamage) - (target.defense + target.extraDefense);
        if (finalAttackDamage <= 0)
        {
            finalAttackDamage = 1;
        }
        bool isDodged = false;
        if (Random.Range(characterStats.dodgeRate + characterStats.extraDodgeRate, 100) == 1)
        {
            isDodged = true;
        }
        if (isDodged == false)
        {
            if (attackID == -1)
            {
                characterStats.currentMP += 10;
                if (characterStats.currentMP > characterStats.maxMP)
                {
                    characterStats.currentMP = characterStats.maxMP;
                }
                target.currentHP -= finalAttackDamage;
            }
            else
            {
                switch (ID)
                {
                    case 0:
                        if (attackID == 0)
                        {
                            target.currentHP -= finalAttackDamage;
                        }
                        break;
                }
            }
            target.AddEffect(2, characterStats.element);
            target.sceneCharacter.isHit();
        }
    }

    IEnumerator WaitOption()
    {
        database.isSelectedOption = false;
        database.selector = database.allyDetails.IndexOf(characterStats.gameObject);
        battleMenu.Show();
        yield return new WaitUntil(() => database.isSelectedOption == true);
        database.isSelectedOption = false;
        switch (database.selectedState)
        {
            case 0:
                performAttackPattern(0, -1, database.selectedIndex, false);
                break;
            case 1:
                Debug.Log("Caster: " + database.selector + ", Skill: " + database.allyDetails[database.selector].GetComponent<Character>().skills[database.selectedItem].skillName + ", Is Ally Side: " + database.isAllySelected + ", Target Index: " + database.selectedIndex);
                break;
            case 2:
                Debug.Log("Caster: " + database.selector + ", Item: " + database.inventory[database.selectedItem].itemName + ", Is Ally Side: " + database.isAllySelected + ", Target Index: " + database.selectedIndex);
                database.inventory[database.selectedItem].itemAmount--;
                if (database.inventory[database.selectedItem].itemAmount <= 0)
                {
                    database.inventory.RemoveAt(database.selectedItem);
                }
                break;
        }
        StartCoroutine("playAnimation");
    }
}
