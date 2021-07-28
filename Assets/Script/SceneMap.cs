using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMap : MonoBehaviour
{
    public int sceneNumber, foregroundID;
    public bool isBackground = false;
    public Sprite[] backgroundMap, foregoundMap;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)transform.position.z * 1;
        transform.position = Vector3.zero;
        if (isBackground == false)
        {
            transform.position += new Vector3(0, 0, -1);
            sr.sprite = foregoundMap[sceneNumber];
            if (foregroundID == 1)
            {
                transform.position += new Vector3(5, 0, 0);
            }
        }
        else
        {
            sr.sprite = backgroundMap[sceneNumber];
        }
    }
}
