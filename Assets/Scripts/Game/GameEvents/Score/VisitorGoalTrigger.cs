
using Assets.Scripts.Game.GameEvents.Commons;
using Assets.Scripts.Game.GameEvents.Score;
using Unity.Netcode;
using UnityEngine;

public class VisitorGoalTrigger : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameTags.Ball.ToString()))
        {
            ScoreEvents.OnVisitorGoalScored?.Invoke();
        }
    }
}
