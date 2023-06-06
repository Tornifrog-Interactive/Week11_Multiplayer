using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTag : NetworkBehaviour {

    [Networked(OnChanged = nameof(PlayerTagChanged))]
    public string ObjectTag { get; set; }
    private static void PlayerTagChanged(Changed<PlayerTag> changed) {
        changed.Behaviour.tag = changed.Behaviour.ObjectTag;
    }
}
