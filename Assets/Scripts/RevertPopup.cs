using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevertPopup : MonoBehaviour
{
	public Image backgroundImage;
	public Button yesRevertButton, noRevertButton;
	public Text yesText, noText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ButtonsInit(Action firstAction)
	{
		yesRevertButton.onClick.AddListener(() =>
		{
			firstAction?.Invoke();
			RemoveThis();
		});
		noRevertButton.onClick.AddListener(() =>
		{
			RemoveThis();
		});
	}

	public void RemoveThis()
	{
		Destroy(this.gameObject);
	}
}
