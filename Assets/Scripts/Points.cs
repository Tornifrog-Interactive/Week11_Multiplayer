using Fusion;
using UnityEngine;

public class Points : NetworkBehaviour
{
    [SerializeField] NumberField PointsDisplay;

    [Networked(OnChanged = nameof(NetworkedPointsChanged))]
    public int NetworkedPoints { get; set; } = 0;
    private static void NetworkedPointsChanged(Changed<Points> changed) {
        Debug.Log($"Points changed to: {changed.Behaviour.NetworkedPoints}");
        changed.Behaviour.PointsDisplay.SetNumber(changed.Behaviour.NetworkedPoints);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // All players can call this function; only the StateAuthority receives the call.
    public void GetPointsRpc(int points) {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received GetPointsRpc on StateAuthority, modifying Networked variable");
        NetworkedPoints += points;
    }
}
