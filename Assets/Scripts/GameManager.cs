using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playersReady = 0;
    public int playersEntity = 0;
    public int playersHumans = 0;

    public int maxPlayersEntity = 1;
    public int maxPlayersHumans = 4;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
