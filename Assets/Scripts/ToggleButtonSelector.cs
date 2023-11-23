using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleButtonSelector : NetworkBehaviour
{
    [SerializeField]
    private GameObject _frameSelect;

    [SerializeField]
    private TextMeshProUGUI entityText;

    [SerializeField]
    private TextMeshProUGUI humanText;

    [SerializeField]
    private ClientDataPlayer clientDataPlayer;

    private Button _button;

    public bool isActivated;

    private void OnEnable()
    {
        LobbyManager.Instance.OnEntityValueChanged += UpdateEntityTextValues;
        LobbyManager.Instance.OnHumansValueChanged += UpdateHumansTextValues;
    }

    private void OnDisable()
    {
        LobbyManager.Instance.OnEntityValueChanged -= UpdateEntityTextValues;
        LobbyManager.Instance.OnHumansValueChanged -= UpdateHumansTextValues;
    }

    // private void Start()
    // {
    //     _button = GetComponent<Button>();
    // }

    public override void OnStartClient()
    {
        if (!isOwned)
        {
            return;
        }
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!isOwned)
        {
            return;
        }
        humanText.text = LobbyManager.Instance.playersHumans.ToString();
        entityText.text = LobbyManager.Instance.playersEntity.ToString();
    }

    public void ChangeHumanValue()
    {
        ActiveChanger();
        if (isActivated)
        {
            LobbyManager.Instance.UpdateSelectedHumans(LobbyManager.Instance, 1, this);
            clientDataPlayer.isEntity = false;
        }
        else
            LobbyManager.Instance.UpdateSelectedHumans(LobbyManager.Instance, -1, this);
    }

    public void ChangeEntityValue()
    {
        ActiveChanger();
        if (isActivated)
        {
            LobbyManager.Instance.UpdateSelectedEntity(LobbyManager.Instance, 1, this);
            clientDataPlayer.isEntity = true;
        }
        else
            LobbyManager.Instance.UpdateSelectedEntity(LobbyManager.Instance, -1, this);
    }

    public void ChangeReadyValue()
    {
        ActiveChanger();
        if (isActivated)
            LobbyManager.Instance.UpdateReadyPlayers(LobbyManager.Instance, 1, this);
        else
            LobbyManager.Instance.UpdateReadyPlayers(LobbyManager.Instance, -1, this);
    }

    public void UpdateEntityTextValues(int value)
    {
        this.entityText.text = value.ToString();
    }

    public void UpdateHumansTextValues(int value)
    {
        Debug.Log("Value: " + value.ToString());

        this.humanText.text = value.ToString();
    }

    public void ActiveChanger()
    {
        isActivated = !isActivated;
        ToggleFrameSelection();
    }

    public void ToggleFrameSelection()
    {
        _frameSelect.SetActive(isActivated);
    }

    public void EnableButton()
    {
        //_button.interactable = true;
    }

    public void DisableButton()
    {
        //_button.interactable = false;
    }
}
