using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
	public TextMeshProUGUI playerNameLabel;
	public TextMeshProUGUI initiativeAndTickLabel;
	public Image icon, activePlayerFrame;
	public Image backgroundImage;
	public AddPopup popup;
	public int ActiveListIndex;
	public static int counter = 0;
	public string playerName;
	public int initiative; //Will determine acting tick except when waiting
	public int actingTick; //Determines when the player becomes the active player
	public bool hasTurnEnded, isNotWaiting, isAnActivePlayer;

	private InitiativeTrackerMenu Menu;
	private AddPopup thePopup;
	private bool isInitiativeEqualsTick { get { return initiative == actingTick; } }



	private void Awake()
	{
		
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void EntryInit(InitiativeTrackerMenu InitiativeMenu)
	{
		Menu = InitiativeMenu;
		counter++;
		SetName("Test" + counter.ToString());
		SetInitiative(counter);
		SetActingTick(initiative);
		hasTurnEnded = false;
		isNotWaiting = true;
		isAnActivePlayer = false;
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}
	public void OnClick()
	{
		Menu.OnClickPlayerEntry(this);
		OpenAddPopup();
	}

	public void OpenAddPopup()
	{
		AddPopup popupInstance = Instantiate(popup, GetComponentInParent<Canvas>().transform);
		popupInstance.PopupInit(this, Menu);
		thePopup = popupInstance;
	}

	public void SetName(string name)
	{
		playerName = name;
		playerNameLabel.text = name;
	}

	public void SetInitiative(int initiative)
	{
		this.initiative = initiative;
		if (isNotWaiting) // only set actingtick equals to initiative when changing initiative if you're not waiting
		{
			SetActingTick(initiative);
		}
  
		initiativeAndTickLabel.text = isInitiativeEqualsTick ? initiative.ToString() : $"<color=#DB2828>{initiative.ToString()}</color>/<color=#005500>{actingTick.ToString()}</color>";
	}
	
	public void SetActingTick(int actingTick)
	{
		this.actingTick = actingTick;
		initiativeAndTickLabel.text = isInitiativeEqualsTick ? initiative.ToString() : $"<color=#DB2828>{initiative.ToString()}</color>/<color=#005500>{this.actingTick.ToString()}</color>";
		if (thePopup != null)
		{
			thePopup.actingTickInput.text = actingTick.ToString();
		}
		if (!isNotWaiting && actingTick == initiative) // don't 'unwait' when changing actingTick unless it is the same as your initiative anyway
		{
			SetIsWaiting(false);
		}
	}

	public void SetPlayerBgColor(Color newColor)
	{
		backgroundImage.color = newColor;
	}

	public void SetPlayerIcon(Sprite sprite)
	{
		icon.sprite = sprite;
	}

	public void SetHasTurnEnded(bool hasTurnEnded)
	{
		this.hasTurnEnded = hasTurnEnded;
		if (hasTurnEnded) {
			SetPlayerBgColor(new Color(0.72f, 0.6666f, 0.46f));
		} else {
			SetPlayerBgColor(new Color(0.86f, 0.81f, 0.66f));
		}
	}

	public void SetActivePlayer(bool isActive)
	{
		isAnActivePlayer = isActive;
		SetActivePlayerFrame(isActive);
		if (isActive)
		{
			SetIsWaiting(false);
		}
	}

	public void SetActivePlayerFrame(bool shouldFrameBeVisible)
	{
		activePlayerFrame.gameObject.SetActive(shouldFrameBeVisible);
	}

	public void SetIsWaiting(bool isWaiting)
	{
		isNotWaiting = !isWaiting;
		if (thePopup != null)
		{
			thePopup.ToggleIsWaiting.isOn = !isNotWaiting;
		}

	}

}
