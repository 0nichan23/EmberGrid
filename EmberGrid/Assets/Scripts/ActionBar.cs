using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private Button attackButton; //open up attack selection
    [SerializeField] private Button waitButton; //execute wait action

    private void Start()
    {
        GameManager.Instance.SelectionManager.OnSelectUnit.AddListener(Setup);
        GameManager.Instance.SelectionManager.OnDeselectedUnit.AddListener(OnDeselect);
        gameObject.SetActive(false);
    }

    private void OnDeselect(Unit given)
    {
        gameObject.SetActive(false);
    }


    private void Setup(Unit given)
    {
        waitButton.onClick.RemoveAllListeners();
        attackButton.onClick.RemoveAllListeners();

        if (ReferenceEquals(given, null))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            //attack button setup
            attackButton.onClick.AddListener(given.TestAttack);
            waitButton.onClick.AddListener(given.ActionHandler.TakeWaitAction);
        }
    }


}
