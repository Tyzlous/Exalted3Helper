using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class InitiativeTrackerMenu : MonoBehaviour
{
	public GameObject playerList;
	public PlayerEntry playerEntry;
	public TextMeshProUGUI turnText;
	public Canvas canvas;
	
	private List<PlayerEntry> playerEntryList = new List<PlayerEntry>();
	private List<PlayerEntry> activePlayerList = new List<PlayerEntry>();
	private int turnCounter;
	private enum SortMode
	{
		HideNone,
		HideTurnEnded,
	}

	private SortMode sortMode = SortMode.HideNone; 
	
	// Start is called before the first frame update
	void Start()
	{
		turnCounter = 1;
		turnText.text = "Setup";
		Application.targetFrameRate = 60;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void DeleteEntry(PlayerEntry player)
	{
		RemoveActivePlayer(player);
		playerEntryList.Remove(player);
		SortPlayerList();
		player.Destroy();
	}
	public void OnClickSort()
	{
		Debug.Log("OnClickSort");
		switch (sortMode)
		{
			case SortMode.HideNone:
				sortMode = SortMode.HideTurnEnded;
				break;
			case SortMode.HideTurnEnded:
				sortMode = SortMode.HideNone;
				break;
			default:
				break;
		}
		SortPlayerList();
	}

	public void OnClickAdd()
	{
		Debug.Log("OnClickAdd");
		PlayerEntry PlayerEntryClone = Instantiate(playerEntry, playerList.transform);
		PlayerEntryClone.EntryInit(this);
		playerEntryList.Add(PlayerEntryClone);
		PlayerEntryClone.OpenAddPopup();
	}

	public void OnClickTick()
	{
		Debug.Log("OnClickTick");
		turnText.text = $"Turn {turnCounter}";
		Tick();
	}

	public void Tick()
	{
		if (activePlayerList != null && activePlayerList.Count != 0) //Clean up active players, if any
		{
			for (int i = 0; i < activePlayerList.Count; i++)
			{
				activePlayerList[i].SetActivePlayer(false);
				if (activePlayerList[i].isNotWaiting)
				{
					activePlayerList[i].SetHasTurnEnded(true);
				}
				else
				{
					RemoveActivePlayer(activePlayerList[i]);
					i--;
				}
			}
		}
		if (playerEntryList != null && playerEntryList.Count != 0) //Get a new set of active players, if any are viable
		{
			SortPlayerList();
			activePlayerList = ReturnActivePlayerList(playerEntryList);
			if (activePlayerList.Count == 0)  //If there are no viable active players, initiate new round
			{
				InitiateNewRound();
			}
			for (int i = 0; i < activePlayerList.Count; i++)
			{
				activePlayerList[i].SetActivePlayer(true);
			}
		}
	}

	public void InitiateNewRound()
	{
		turnCounter++;
		turnText.text = $"Turn {turnCounter}";
		for (int i = 0; i < playerEntryList.Count; i++)
		{
			playerEntryList[i].SetHasTurnEnded(false);
			playerEntryList[i].SetActingTick(playerEntryList[i].initiative);
		}
		sortMode = SortMode.HideNone;
		SortPlayerList();
		activePlayerList = ReturnActivePlayerList(playerEntryList);
	}

	/// <summary>
	/// Assuming list is correctly ordered in descending order
	/// </summary>
	public List<PlayerEntry> ReturnActivePlayerList(List<PlayerEntry> playerEntryList)
	{
		if (playerEntryList == null || playerEntryList.Count == 0)
		{
			return new List<PlayerEntry>();
		}

		bool isViablePlayerFound = false;
		List<PlayerEntry> activePlayerList = new List<PlayerEntry>();
		for (int i = 0; i < playerEntryList.Count; i++)
		{
			if (isViablePlayerFound)
			{
				if (!playerEntryList[i].hasTurnEnded && playerEntryList[i].actingTick == activePlayerList[0].actingTick)
				{
					activePlayerList.Add(playerEntryList[i]);
				}
			}
			else if (!playerEntryList[i].hasTurnEnded && !playerEntryList[i].isAnActivePlayer)
			{
				activePlayerList.Add(playerEntryList[i]);
				isViablePlayerFound = true;
			}
		}
		RefreshActivePlayerIndexes();
		return activePlayerList;
	}

	public void AddToActivePlayerList(PlayerEntry newActivePlayer)
	{
		activePlayerList.Add(newActivePlayer);
		newActivePlayer.SetActivePlayer(true);
		RefreshActivePlayerIndexes();
	}

	public void RemoveActivePlayer(PlayerEntry entryToRemoveFromList)
	{
		entryToRemoveFromList.SetActivePlayer(false);
		activePlayerList.Remove(entryToRemoveFromList);
		RefreshActivePlayerIndexes();

	}

	public void RefreshActivePlayerIndexes()
	{
		for (int i = 0; i < activePlayerList.Count; i++)
		{
			activePlayerList[i].ActiveListIndex = i;
		}
	}

	public void OnClickOptions()
	{
		Debug.Log("OnClickOptions");
	}

	public void OnClickPlayerEntry(PlayerEntry EntryPressed)
	{
		Debug.Log("OnClickPlayerEntry" + EntryPressed.playerName);
	}

	public void SortPlayerList()
	{
		playerEntryList = playerEntryList.OrderByDescending(playerEntry => playerEntry.actingTick).ToList();
		for (int i = 0; i < playerEntryList.Count; i++)
		{
			playerEntryList[i].transform.SetSiblingIndex(i);
		}
		if (sortMode == SortMode.HideNone)
		{
			for (int i = 0; i < playerEntryList.Count; i++)
			{
					playerEntryList[i].gameObject.SetActive(true);
			}
		}
		else if (sortMode == SortMode.HideTurnEnded)
		{
			for (int i = 0; i < playerEntryList.Count; i++)
			{
				if (playerEntryList[i].hasTurnEnded)
				{
					playerEntryList[i].gameObject.SetActive(false);
				}
			}
		}
	}
}
