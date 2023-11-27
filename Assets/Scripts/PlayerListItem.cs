using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using TMPro;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int ConnectionID;
    public ulong PlayerSteamID;
    private bool AvatarRecieved;

    public NetworkConnection playerConnection;

    public TextMeshProUGUI PlayernameTMP;
    public RawImage PlayerIcon;

    public TextMeshProUGUI PlayerReadyTMP;
    public bool Ready;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    public void ChangeReadyStatus()
    {
        if (Ready)
        {
            PlayerReadyTMP.text = "Ready";
            PlayerReadyTMP.color = Color.green;
        }
        else
        {
            PlayerReadyTMP.text = "Unready";
            PlayerReadyTMP.color = Color.red;
        }
    }

    private void Start()
    {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID == PlayerSteamID)
        {
            PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else // another player
        {
            return;
        }
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        AvatarRecieved = true;
        return texture;
    }

    private void GetPlayericon()
    {
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if (ImageID == -1)
        {
            return;
        }
        PlayerIcon.texture = GetSteamImageAsTexture(ImageID);
    }

    public void SetPlayerValues()
    {
        PlayernameTMP.text = playerName;
        ChangeReadyStatus();
        if (!AvatarRecieved)
        {
            GetPlayericon();
        }
    }
}