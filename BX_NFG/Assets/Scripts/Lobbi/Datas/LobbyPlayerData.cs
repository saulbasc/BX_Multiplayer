
using System;
using System.Collections.Generic;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Players.Values;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Players
{
    public class LobbyPlayerData
    {
        private string id;
        private string gameTag;
        private bool isReady;
        private PlayerTeam playerTeam;
        private List<ValueApplier> valueAppliers;

        public string Id => id;
        public string GameTag => gameTag;
        public bool IsReady { set => isReady = value; get => isReady; }
        public PlayerTeam PlayerTeam { set => playerTeam = value; get => playerTeam; }

        private class ValueApplier
        {
            public string Key;
            public IValueGetter Getter;
            public Action<object> Apply;
        }

        public LobbyPlayerData(string id, string gameTag)
        {
            this.id = id;
            this.gameTag = gameTag;
            isReady = false;
            playerTeam = PlayerTeam.Spectator;
        }

        public LobbyPlayerData(Dictionary<string, PlayerDataObject> playerData)
        {
            InizialiceValueAppliers();
            UpdateState(playerData);
        }

        private void InizialiceValueAppliers()
        {
            valueAppliers = new List<ValueApplier>
            {
                new ValueApplier
                {
                    Key = PlayerDataKeys.Id,
                    Getter = new StringValueGetter(),
                    Apply = value => id = (string)value
                },
                new ValueApplier
                {
                    Key = PlayerDataKeys.GameTag,
                    Getter = new StringValueGetter(),
                    Apply = value => gameTag = (string)value
                },
                new ValueApplier
                {
                    Key = PlayerDataKeys.IsReady,
                    Getter = new BoolValueGetter(),
                    Apply = value => isReady = (bool)value
                },
                new ValueApplier
                {
                    Key = PlayerDataKeys.PlayerTeam,
                    Getter = new EnumValueGetter<PlayerTeam>(),
                    Apply = value => playerTeam = (PlayerTeam)value
                }
            };
        }

        private void UpdateState(Dictionary<string, PlayerDataObject> playerData)
        {
            foreach (var applier in valueAppliers)
            {
                if (playerData.ContainsKey(applier.Key))
                {
                    object value = applier.Getter.GetValue(playerData, applier.Key);
                    applier.Apply(value);
                }
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                { PlayerDataKeys.Id, id },
                { PlayerDataKeys.GameTag, gameTag },
                { PlayerDataKeys.IsReady, isReady.ToString() },
                { PlayerDataKeys.PlayerTeam, playerTeam.ToString() },
            };
        }
    }
}
