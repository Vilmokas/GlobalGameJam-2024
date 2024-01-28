using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private TextMeshProUGUI selectedJesterCharismaText;
    [SerializeField] private TextMeshProUGUI selectedJesterIntelligenceText;
    [SerializeField] private TextMeshProUGUI selectedJesterStrengthText;
    [SerializeField] private TextMeshProUGUI selectedJesterAgilityText;

    private Subject selectedSubject = null;
    [SerializeField] private Color newSubjectColor = Color.white;
    private Vector3 oldItemPosition = Vector3.zero;
    [SerializeField] private Transform performingItemPosition;

    [SerializeField] private TextMeshProUGUI diceText;
    [SerializeField] private TextMeshProUGUI rollNeededText;
    [SerializeField] private TextMeshProUGUI rollBonusText;
    [SerializeField] private GameObject dice;

    [SerializeField] private int laughsNeeded;
    [SerializeField] private int laughsCurrent;
    [SerializeField] private Slider laughMeter;
    [SerializeField] private int jesterCount;

    [SerializeField] private GameObject jokePanel;
    [SerializeField] private TextMeshProUGUI jokeText;

    [SerializeField] private SceneController sceneController;

    public enum SubjectTypes
    {
        crown,
        robes,
        scepter,
        walls,
        jester,
        banquet,
        trumpet,
        garden,
        chalice,
        tapestry
    }

    public List<String> goodJokes;
    public List<String> badJokes;

    private bool gamePaused = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        laughMeter.maxValue = laughsNeeded;
        laughMeter.value = 0;
    }

    public void SelectJester(Jester jester)
    {
        if (gamePaused) return;

        if (selectedJester != null)
        {
            selectedJester.transform.position = oldJesterPosition;
        }
        selectedJester = jester;
        oldJesterPosition = jester.transform.position;
        jester.transform.position = performingJesterPosition.position;

        selectedJesterCharismaText.text = selectedJester.charisma.ToString();
        selectedJesterIntelligenceText.text = selectedJester.intelligence.ToString();
        selectedJesterStrengthText.text = selectedJester.strength.ToString();
        selectedJesterAgilityText.text = selectedJester.agility.ToString();

        SoundController.Instance.PlayAudioClip(0);
    }

    public void SelectSubject(Subject subject)
    {
        if (gamePaused) return;

        if (selectedSubject != null)
        {
            selectedSubject.transform.position = oldItemPosition;
        }
        selectedSubject = subject;
        oldItemPosition = subject.transform.position;
        if (subject.subjectType != SubjectTypes.crown) subject.transform.position = performingItemPosition.position;

        SoundController.Instance.PlayAudioClip(0);
    }

    public void RollDice()
    {
        if (gamePaused) return;
        if (selectedJester == null) return;
        if (selectedSubject == null) return;

        int rollNeeded = 0;
        int rollBonus = 0;

        if (selectedSubject.charisma > rollNeeded)
        {
            rollNeeded = selectedSubject.charisma;
            rollBonus = selectedJester.charisma;
        }
        if (selectedSubject.intelligence > rollNeeded)
        {
            rollNeeded = selectedSubject.intelligence;
            rollBonus = selectedJester.intelligence;
        }
        if (selectedSubject.strength > rollNeeded)
        {
            rollNeeded = selectedSubject.strength;
            rollBonus = selectedJester.strength;
        }
        if (selectedSubject.agility > rollNeeded)
        {
            rollNeeded = selectedSubject.agility;
            rollBonus = selectedJester.agility;
        }

        rollNeededText.text = rollNeeded.ToString();
        rollBonusText.text = rollBonus.ToString();

        int newRoll = UnityEngine.Random.Range(0, 21);
        int newRollWithBonus = newRoll + rollBonus;

        SoundController.Instance.PlayAudioClip(2);


        if (newRollWithBonus > rollNeeded)
        {
            Debug.Log("pass");
            jokeText.text = goodJokes[(int)selectedSubject.subjectType];
            StartCoroutine(HideJokePanel(newRollWithBonus, false));
        }
        else
        {
            Debug.Log("fail");
            jokeText.text = badJokes[(int)selectedSubject.subjectType];
            StartCoroutine(HideJokePanel(newRollWithBonus, true));
        }
    }

    IEnumerator HideJokePanel(int roll, bool fail)
    {
        gamePaused = true;
        dice.SetActive(true);
        diceText.text = UnityEngine.Random.Range(0, 21).ToString();
        yield return new WaitForSeconds(0.05f);
        diceText.text = UnityEngine.Random.Range(0, 21).ToString();
        yield return new WaitForSeconds(0.05f);
        diceText.text = UnityEngine.Random.Range(0, 21).ToString();
        yield return new WaitForSeconds(0.1f);
        diceText.text = UnityEngine.Random.Range(0, 21).ToString();
        yield return new WaitForSeconds(0.1f);
        diceText.text = UnityEngine.Random.Range(0, 21).ToString();
        yield return new WaitForSeconds(0.2f);
        diceText.text = UnityEngine.Random.Range(0, 21).ToString();
        yield return new WaitForSeconds(0.2f);

        diceText.text = roll.ToString();
        yield return new WaitForSeconds(3);
        dice.SetActive(false);

        jokePanel.SetActive(true);
        SoundController.Instance.PlayAudioClip(3);
        yield return new WaitForSeconds(8);
        jokePanel.SetActive(false);

        if (fail)
        {
            SoundController.Instance.PlayAudioClip(5);
            yield return new WaitForSeconds(1);
            Destroy(selectedJester.gameObject);
            selectedJester = null;
            jesterCount--;

            SoundController.Instance.PlayAudioClip(1);

            if (jesterCount == 0)
            {
                yield return new WaitForSeconds(0.5f);
                Debug.Log("level failed");
                SoundController.Instance.PlayAudioClip(7);
                sceneController.ShowGameEndWindow(false);
            }
        }
        else
        {
            SoundController.Instance.PlayAudioClip(4);

            if (selectedSubject.subjectType != SubjectTypes.crown)
            {
                Destroy(selectedSubject.gameObject);
                selectedSubject = null;
            }

            laughsCurrent += 1;
            laughMeter.value = laughsCurrent;

            if (laughsCurrent >= laughsNeeded)
            {
                yield return new WaitForSeconds(0.5f);
                Debug.Log("level complete");
                SoundController.Instance.PlayAudioClip(6);
                sceneController.ShowGameEndWindow(true);
            }
        }

        gamePaused = false;
    }
}
