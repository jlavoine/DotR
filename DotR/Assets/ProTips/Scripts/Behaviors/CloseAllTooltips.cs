using UnityEngine;
using UnityEngine.UI;

namespace ModelShark
{
    /// <summary>Purpose of this script is to close down all open tooltips with the push of a button. Put this script on a Button UI object.</summary>
    [RequireComponent(typeof(Button))]
    public class CloseAllTooltips : MonoBehaviour
    {
        private void Start()
        {
            // Get the button on this object.
            Button button = gameObject.GetComponent<Button>();

            // Wireup the button's OnClick event.
            button.onClick.AddListener(()=>TooltipManager.Instance.CloseAll());
        }
    }
}