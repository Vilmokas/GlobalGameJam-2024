using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    private Color originalColor;
    [SerializeField] public GameManager.SubjectTypes subjectType;
    [SerializeField] public int charisma;
    [SerializeField] public int intelligence;
    [SerializeField] public int strength;
    [SerializeField] public int agility;

    private void Start()
    {
        originalColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
    }

    public void ChangeSpriteColor(Color color)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = color;
    }

    public void ChangeSpriteColor()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = originalColor;
    }

    private void OnMouseDown()
    {
        GameManager.Instance.SelectSubject(this);
    }
}
