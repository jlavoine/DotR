using UnityEngine;

namespace ModelShark
{
    public class ButtonEventsForDemo : MonoBehaviour
    {
        public void BackToDemo()
        {
            Application.LoadLevel("ProTips Demo");
        }

        public void GoToTemplates()
        {
            Application.LoadLevel("Tooltip Workshop");
        }
    }
}