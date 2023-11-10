using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ToggleButtonSelector : NetworkBehaviour
{
    [SerializeField] private GameObject _frameSelect;
    public bool isActivated;

    private Button _button;

    [SerializeField] private TextMeshProUGUI entityText;
    [SerializeField] private TextMeshProUGUI humanText;

    //[SerializeField] private LobbyManager lobbyManager;

    private void OnEnable()
    {
        //LobbyManager.Instance.OnEntityValueChanged += UpdateEntityTextValues;
        //  LobbyManager.Instance.OnHumansValueChanged += UpdateHumansTextValues;
        LobbyManager.Instance.OnHumansValueChanged += UpdateHumansTextValues;
    }
    private void OnDisable()
    {
        // LobbyManager.Instance.OnEntityValueChanged -= UpdateEntityTextValues;
        LobbyManager.Instance.OnHumansValueChanged -= UpdateHumansTextValues;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            //   GetComponent<ToggleButtonSelector>().enabled = false;
        }
    }
    private void Start()
    {
        Debug.Log("Кнопка получена");
           _button = GetComponent<Button>();

    }
    private void Update()
    {
        this.humanText.text = LobbyManager.Instance.playersHumans.ToString();

    }
    public void ActiveChanger()
    {
        isActivated = !isActivated;
        ToggleFrameSelection();
    }
    public void ChangeHumanValue()
    {
        ActiveChanger();
        Debug.Log("Отослал чел");
        Debug.Log(LobbyManager.Instance);
        if (isActivated)
            LobbyManager.Instance.UpdateSelectedHumans(LobbyManager.Instance, 1, this);
        else
            LobbyManager.Instance.UpdateSelectedHumans(LobbyManager.Instance, -1, this);

    }

    public void ChangeEntityValue()
    {
        Debug.Log("Отослал энт");

        ActiveChanger();
        if (isActivated)
            LobbyManager.Instance.UpdateSelectedEntity(LobbyManager.Instance, 1, this);
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
    public void ToggleFrameSelection()
    {
        _frameSelect.SetActive(isActivated);
    }
    public void EnableButton()
    {
        _button.interactable = true;
    }
    public void DisableButton()
    {
        _button.interactable = false;
    }
    public void UpdateEntityTextValues(int value)
    {
        this.entityText.text = value.ToString();
    }
    public void UpdateHumansTextValues(int value)
    {
        Debug.Log("Текст должен был поменяться");
        Debug.Log("Переменная: " + LobbyManager.Instance.playersHumans.ToString());
        Debug.Log("Value: " + value.ToString());

        this.humanText.text = value.ToString();

    }
}
