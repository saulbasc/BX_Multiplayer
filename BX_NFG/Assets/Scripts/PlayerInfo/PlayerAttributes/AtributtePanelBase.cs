
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerInfo.PlayerAttributes
{
    abstract class AtributtePanelBase
    {
        [SerializeField] private TextMeshProUGUI AttributeNameText;
        [SerializeField] private Button minButton;
        [SerializeField] private Button maxButton;
    }
}
