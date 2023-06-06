using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined {
    public GameObject PlayerPrefab;

    public List<string> GroupTags = new() { "BlueTeam", "RedTeam" };

    public List<Color> GroupColors = new() { Color.blue, Color.red };    
    public enum Groups
    {
        BlueTeam = 0,
        RedTeam
    };

    private Dictionary<int, string> PlayerIdToTeam = new Dictionary<int, string>();

    private static int _BlueTeamBalance = 0;
    
    public void PlayerJoined(PlayerRef player) {
        if (player == Runner.LocalPlayer) {
            var playerObject = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
            var numberOfActivePlayers = Runner.ActivePlayers.Count();
            var isBlueTeam = numberOfActivePlayers % 2 == 0; 
            playerObject.GetComponent<PlayerColor>().NetworkedColor = isBlueTeam
                ? GroupColors[(int)Groups.BlueTeam]
                : GroupColors[(int)Groups.RedTeam]; 
            var playerTag = isBlueTeam
                ? GroupTags[(int)Groups.BlueTeam]
                : GroupTags[(int)Groups.RedTeam];
            playerObject.gameObject.GetComponent<PlayerTag>().ObjectTag = playerTag;
            if (isBlueTeam)
            {
                _BlueTeamBalance--;
            }
            else
            {
                _BlueTeamBalance++;
            }
            
            PlayerIdToTeam.Add(player.PlayerId, playerTag);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var playerTag = PlayerIdToTeam[player.PlayerId];
            if (playerTag == GroupTags[(int)Groups.BlueTeam])
                _BlueTeamBalance++;
            else
                _BlueTeamBalance--;
        }
    }
}