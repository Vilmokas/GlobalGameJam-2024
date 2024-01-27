using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jester : MonoBehaviour
{
    [SerializeField] public int charisma;
    [SerializeField] public int intelligence;
    [SerializeField] public int strength;
    [SerializeField] public int agility;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameManager.Instance.SelectJester(this);
    }
}
