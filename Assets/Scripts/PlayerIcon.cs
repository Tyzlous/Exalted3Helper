using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
	public Button iconButton;
	public Image iconImage;
	public GameObject imageHighLight;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetHighlight(bool isOn)
	{
		imageHighLight.SetActive(isOn);
	}
}
