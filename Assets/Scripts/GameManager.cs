using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null");

            }
            return _instance;
        }
    }

    private Jester selectedJester = null;
    [SerializeField] private Vector3 oldJesterPosition = Vector3.zero;
    [SerializeField] private Transform performingJesterPosition;

    private Subject selectedSubject = null;
    [SerializeField] private Color newSubjectColor = Color.white;

    [SerializeField] private TextMeshProUGUI diceText;
    [SerializeField] private TextMeshProUGUI rollNeededText;
    [SerializeField] private TextMeshProUGUI rollBonusText;
    public enum SubjectTypes
    {
        king,
        queen,
        guard,
        flower,
    }

    private void Awake()
    {
        _instance = this;
    }

    public void SelectJester(Jester jester)
    {
        if (selectedJester != null)
        {
            selectedJester.transform.position = oldJesterPosition;
        }
        selectedJester = jester;
        oldJesterPosition = jester.transform.position;
        jester.transform.position = performingJesterPosition.position;
    }

    public void SelectSubject(Subject subject)
    {
        if (selectedSubject != null)
        {
            selectedSubject.ChangeSpriteColor();
        }
        selectedSubject = subject;
        selectedSubject.ChangeSpriteColor(newSubjectColor);
    }

    public void RollDice()
    {
        if (selectedJester == null) return;
        if (selectedSubject == null) return;

        int rollNeeded = 0;
        int rollBonus = 0;

        if (selectedSubject.charisma > rollNeeded)
        {
            rollNeeded = selectedSubject.charisma;
            rollBonus = selectedJester.charisma;
        }
        else if (selectedSubject.intelligence > rollNeeded)
        {
            rollNeeded = selectedSubject.intelligence;
            rollBonus = selectedJester.intelligence;
        }
        else if (selectedSubject.strength > rollNeeded)
        {
            rollNeeded = selectedSubject.strength;
            rollBonus = selectedJester.strength;
        }
        else
        {
            rollNeeded = selectedSubject.agility;
            rollBonus = selectedJester.agility;
        }

        rollNeededText.text = rollNeeded.ToString();
        rollBonusText.text = rollBonus.ToString();

        int newRoll = UnityEngine.Random.Range(0, 21);
        int newRollWithBonus = newRoll + rollBonus;

        diceText.text = newRollWithBonus.ToString();

        Debug.Log(newRollWithBonus > rollNeeded ? "passed" : "failed");
    }
}
