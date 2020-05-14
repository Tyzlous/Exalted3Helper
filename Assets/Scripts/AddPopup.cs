using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AddPopup : MonoBehaviour
{
	private class PlayerEntryBackup
	{
		public int backupInitiative, backupActingTick;
		public string backupName;
		public bool backupIsNotWaiting, backupHasTurnEnded, backupIsAnActivePlayer;
		public Sprite backupPlayerIcon;
	}
	public PlayerEntry playerEntry;
	public PlayerIcon PlayerIcon;
	public GameObject imageListHorizontal;
	public RevertPopup revertPopup;
	public Button playerImage;
	public List<Sprite> iconList;
	public TMP_InputField nameInput, initiativeInput, actingTickInput;
	public TextMeshProUGUI tickInputPlaceholderText;
	public Button acceptButton;
	public Canvas canvas;
	public Toggle ToggleIsWaiting, ToggleEndedTurn, ToggleIsActivePlayer;
	
	private InitiativeTrackerMenu Menu;
	private List<PlayerIcon> playerIconList = new List<PlayerIcon>();
	private PlayerEntryBackup backupData = new PlayerEntryBackup();
	
	// Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//ResetToBackupData();
			AcceptChanges();
			Destroy(this.gameObject);
		}
    }
	public void PopupInit(PlayerEntry Entry, InitiativeTrackerMenu Menu)
	{
		playerEntry = Entry;
		this.Menu = Menu;
		ImageListInit();
		nameInput.text = playerEntry.playerName;
		actingTickInput.text = playerEntry.actingTick.ToString();
		initiativeInput.text = playerEntry.initiative.ToString();
		ToggleIsWaiting.isOn = !playerEntry.isNotWaiting;
		ToggleEndedTurn.isOn = playerEntry.hasTurnEnded;
		ToggleIsActivePlayer.isOn = playerEntry.isAnActivePlayer;
		CreateBackupData();

	}
	public void CreateBackupData()
	{
		backupData.backupName = playerEntry.playerName;
		backupData.backupInitiative = playerEntry.initiative;
		backupData.backupActingTick = playerEntry.actingTick;
		backupData.backupHasTurnEnded = playerEntry.hasTurnEnded;
		backupData.backupIsNotWaiting = playerEntry.isNotWaiting;
		backupData.backupIsAnActivePlayer = playerEntry.isAnActivePlayer;
		backupData.backupPlayerIcon = playerEntry.icon.sprite;
	}
	public void ResetToBackupData()
	{
		playerEntry.SetName(backupData.backupName);
		playerEntry.SetHasTurnEnded(backupData.backupHasTurnEnded);
		if (backupData.backupIsAnActivePlayer)
		{
			Menu.AddToActivePlayerList(playerEntry);
		}
		else
		{
			Menu.RemoveActivePlayer(playerEntry);
		}
		playerEntry.SetActivePlayer(backupData.backupIsAnActivePlayer);
		playerEntry.SetIsWaiting(!backupData.backupIsNotWaiting);
		playerEntry.SetInitiative(backupData.backupInitiative); 
		playerEntry.SetActingTick(backupData.backupActingTick);
		nameInput.text = playerEntry.playerName;
		actingTickInput.text = playerEntry.actingTick.ToString();
		initiativeInput.text = playerEntry.initiative.ToString();
		ToggleIsWaiting.isOn = !playerEntry.isNotWaiting;
		ToggleEndedTurn.isOn = playerEntry.hasTurnEnded;
		ToggleIsActivePlayer.isOn = playerEntry.isAnActivePlayer;
		playerEntry.icon.sprite = backupData.backupPlayerIcon;
		RefreshHighlightedImages();
	}
	public void RefreshHighlightedImages()
	{
			for (int a = 0; a < playerIconList.Count; a++)
			{
				playerIconList[a].SetHighlight(false);
			}
			for (int i = 0; i < iconList.Count; i++)
			{
				if (playerEntry.icon.sprite == iconList[i] && playerIconList != null && i < playerIconList.Count )
				{
					playerIconList[i].SetHighlight(true);
				}

			}
	}
	public void ImageListInit()
	{
		for (int i = 0; i < iconList.Count; i++)
		{
			PlayerIcon playerIconClone = Instantiate(this.PlayerIcon, imageListHorizontal.transform);
			playerIconClone.iconImage.sprite = iconList[i];
			var icon = iconList[i];
			playerIconClone.iconButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
			{
				Debug.Log("Herpderp" + i.ToString());
				playerEntry.SetPlayerIcon(icon);
				RefreshHighlightedImages();
			}));
			playerIconList.Add(playerIconClone);
		}
			RefreshHighlightedImages();
	}
	public void AcceptChanges()
	{
		SetName(nameInput.text);
		SetInitiative(initiativeInput.text);
		SetActingTick(actingTickInput.text);
		Menu.SortPlayerList();
  
		Destroy(this.gameObject);
	}

	public void DeletePlayer()
	{
		Menu.DeleteEntry(playerEntry);
		Destroy(this.gameObject);
	}
	public void OnClickRevertChanges()
	{
		RevertPopup revertPopupInstance = Instantiate(this.revertPopup, this.transform);
		revertPopupInstance.ButtonsInit(ResetToBackupData);
	}
	public void OnEndEdit()
	{
		SetName(nameInput.text);
		SetInitiative(initiativeInput.text);
		SetActingTick(actingTickInput.text);
	}
	public void SetName(string name)
	{
		playerEntry.SetName(name);
	}
	public void SetInitiative(string initiative)
	{
		if (Int32.TryParse(initiative, out int init))
		{
			playerEntry.SetInitiative(init);
		}
	}
	public void SetActingTick(string tickValue)
	{
		if (Int32.TryParse(tickValue, out int actingTick))
		{
			playerEntry.SetActingTick(actingTick);
		}
	}
	public void SetHasTurnEnded(bool toggleValue)
	{
		playerEntry.SetHasTurnEnded(toggleValue);
	}
	public void SetIsNotWaiting(bool toggleValue)
	{
		if (toggleValue == !playerEntry.isNotWaiting)
		{
			return;
		}
		else
		{
			playerEntry.isNotWaiting = !ToggleIsWaiting.isOn;
		}
	}
	public void SetActivePlayer(bool toggleValue)
	{
		if (toggleValue == playerEntry.isAnActivePlayer)
		{
			return;
		}
		if (toggleValue)
		{
			Menu.AddToActivePlayerList(playerEntry);
		}
		else if (playerEntry.isAnActivePlayer)
		{
			Debug.Log("setactiveplayer else");
			Menu.RefreshActivePlayerIndexes();
			Menu.RemoveActivePlayer(playerEntry);
		}
	}
}