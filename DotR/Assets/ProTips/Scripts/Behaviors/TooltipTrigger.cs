using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModelShark
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector]
        public TooltipStyle tooltipStyle;
        [HideInInspector]
        public List<ParameterizedTextField> parameterizedTextFields;
        [HideInInspector]
        public List<DynamicImageField> dynamicImageFields;
        [HideInInspector]
        public List<DynamicSectionField> dynamicSectionFields;
        
        [HideInInspector] 
        public bool isRemotelyActivated; // Is this tooltip activated from another game object? (ie, NOT "hover" activated)
        [HideInInspector]
        public GameObject remoteTrigger; // The object that's remotely triggering this tooltip, if this tooltip is remotely activated.

        public RectTransform RectTransform { get; private set; }
        
        /// <summary>The tooltip gameobject instance in the scene that matches this trigger's tooltip style.</summary>
        public Tooltip Tooltip { get; set; }
        [Tooltip("Controls the color and fade amount of the tooltip background.")]
        public Color backgroundTint = Color.white;
        public TipPosition tipPosition;
        public int minTextWidth = 100;
        public int maxTextWidth = 200;

        [HideInInspector, Tooltip("Once open, this tooltip will stay open until something manually closes it. No other tooltips that use this style will open, since there is only one instance of a tooltip style at a time.")]
        public bool staysOpen = false;
        [HideInInspector, Tooltip("While open, this tooltip will prevent any other tooltips from triggering.")]
        public bool isBlocking = false;

        // Timer variables - these keep track of how much time has elapsed.
        private float hoverTimer;
        private float popupTimer;
        
        private float tooltipDelay = 0.2f; // How long tooltips delay before showing. This is set on the TooltipManager.
        private float popupTime = 2f; // How long the popup tooltip should remain visible before fading out.
        private bool isInitialized;

        public void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (isInitialized) return;

            RectTransform = gameObject.GetComponent<RectTransform>();

            if (tooltipStyle != null)
            {
                // Check with the TooltipManager to see if there is already a tooltip object instantiated for this style.
                // If there is already a tooltip object in the scene for this style, use it. Otherwise, instantiate it and add it to the TooltipManager.
                if (!TooltipManager.Instance.Tooltips.ContainsKey(tooltipStyle))
                {
                    TooltipStyle ttStyle = Instantiate(tooltipStyle);
                    ttStyle.name = tooltipStyle.name;
                    ttStyle.transform.SetParent(TooltipManager.Instance.RootCanvas.transform, false);
                    Tooltip newTooltip = new Tooltip() { GameObject = ttStyle.gameObject};
                    newTooltip.Initialize();
                    newTooltip.Deactivate();
                    TooltipManager.Instance.Tooltips.Add(tooltipStyle, newTooltip);
                }
                Tooltip = TooltipManager.Instance.Tooltips[tooltipStyle];
            }
            isInitialized = true;
        }

        private void Update()
        {
            tooltipDelay = TooltipManager.Instance.tooltipDelay;

            // Hover timer update
            if (hoverTimer > 0)
                hoverTimer += Time.deltaTime;

            if (hoverTimer > tooltipDelay)
            {
                // Turn off the timer and show the tooltip.
                hoverTimer = 0;
                StartHover();
            }

            // Popup timer update
            if (popupTimer > 0)
                popupTimer += Time.deltaTime;

            if (popupTimer > popupTime && Tooltip != null && !Tooltip.StaysOpen) // Turn off the timer and hide the tooltip.
                HidePopup();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Ignore this event if touch support is on.
            if (TooltipManager.Instance.touchSupport) return;
            // Ignore and exit if this tooltip is remotely activated, if there is already a blocking tooltip on the screen, or if the tooltip is already visible.
            if (isRemotelyActivated || TooltipManager.Instance.BlockingTooltip != null) return;
            if (Tooltip.GameObject.activeInHierarchy) return;

            hoverTimer = 0.001f; // Start the timer.
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Ignore this event is touch support is off.
            if (!TooltipManager.Instance.touchSupport) return;
            // Ignore and exit if this tooltip is remotely activated, if there is already a blocking tooltip on the screen, or if the tooltip is already visible.
            if (isRemotelyActivated || TooltipManager.Instance.BlockingTooltip != null) return;
            if (Tooltip.GameObject.activeInHierarchy) return;

            hoverTimer = 0.001f; // Start the timer.
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            // Ignore this event if touch support is on.
            if (TooltipManager.Instance.touchSupport) return;
            // Ignore and exit if this tooltip is remotely activated, if there is already a blocking tooltip on the screen, or if the tooltip is already visible.
            if (isRemotelyActivated || TooltipManager.Instance.BlockingTooltip != null) return;
            if (Tooltip.GameObject.activeInHierarchy) return;

            hoverTimer = 0.001f; // Start the timer.
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Ignore this event if touch support is on.
            if (TooltipManager.Instance.touchSupport) return;
            StopHover();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Ignore this event if touch support is off.
            if (!TooltipManager.Instance.touchSupport) return;
            StopHover();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            // Ignore this event if touch support is on.
            if (TooltipManager.Instance.touchSupport) return;
            StopHover();
        }

        public void StartHover()
        {
            // Fix if minWidth is greater than maxWidth.
            if (minTextWidth > maxTextWidth)
                maxTextWidth = minTextWidth;

            Tooltip.WarmUp();
            Tooltip.StaysOpen = staysOpen;
            Tooltip.IsBlocking = isBlocking;
            
            TooltipManager.Instance.SetTextAndSize(this);

            // Show and position the tooltip.
            StartCoroutine(TooltipManager.Instance.Show(this));
        }

        /// <summary>Forces the tooltip to be hidden and deactivated, and also resets all timers so the tooltip won't automatically re-enable (unless it's triggered again).</summary>
        public void ForceHideTooltip()
        {
            // Reset all timers to prevent re-showing the tooltip automatically.
            hoverTimer = popupTimer = 0;

            // Reset the text fields and images for the tooltip, and hide it, regardless of how it was triggered (hover over or remote popup, or through code).
            if (Tooltip != null && Tooltip.GameObject != null)
                Tooltip.Deactivate();
        }

        public void StopHover()
        {
            if (Tooltip == null || Tooltip.GameObject == null) return;
            if (isRemotelyActivated || Tooltip.StaysOpen) return;

            hoverTimer = 0; // Stop the timer and prevent the tooltip from showing.

            // Reset the text fields and images for the tooltip, and hide it.
            if (Tooltip != null && Tooltip.GameObject != null)
                Tooltip.Deactivate();
        }

        public void HidePopup()
        {
            popupTimer = 0; // Stop the timer and prevent the tooltip from showing.
            remoteTrigger = null; // reset the object that's remotely triggering this popup.

            // Reset the text fields and images for the tooltip, and hide it.
            if (Tooltip != null && Tooltip.GameObject != null)
                Tooltip.Deactivate();
        }

        /// <summary>Manually pop a tooltip up without requiring hovering. This is useful for in-game tutorials or NPC barks.</summary>
        /// <param name="duration">Number of seconds to display the tooltip.</param>
        /// <param name="triggeredBy">The game object that triggered this popup. Allows us to prevent multiple triggering.</param>
        /// <param name="isBlocking">Does this tooltip block all other tooltips while it is being displayed?</param>
        public void Popup(float duration, GameObject triggeredBy)
        {
            if (popupTimer > 0 || TooltipManager.Instance.BlockingTooltip != null) return;
            
            Initialize();
            remoteTrigger = triggeredBy;

            // Fix if minWidth is greater than maxWidth.
            if (minTextWidth > maxTextWidth)
                maxTextWidth = minTextWidth;

            Tooltip.WarmUp();
            Tooltip.StaysOpen = staysOpen;
            Tooltip.IsBlocking = isBlocking;
            TooltipManager.Instance.SetTextAndSize(this);

            // Show and position the tooltip.
            StartCoroutine(TooltipManager.Instance.Show(this));
            popupTimer = 0.001f; // start the popup timer.
            popupTime = duration; // set the duration of the popup.
        }

        public void SetText(string parameterName, string text)
        {
            // If the list of parameterized text fields doesn't exist, create it.
            if (parameterizedTextFields == null)
                parameterizedTextFields = new List<ParameterizedTextField>();

            // Check to see if we find a matching field. If so, set its text to what was passed in.
            bool fieldExists = false;
            for (int i = 0; i < parameterizedTextFields.Count; i++)
            {
                if (parameterizedTextFields[i].name == parameterName)
                {
                    parameterizedTextFields[i].value = text;
                    fieldExists = true;
                }
            }

            // Finally, if the text field doesn't exist in the parameterized field list, create it and set its text to what was passed in.
            if (!fieldExists)
            {
                string delimiter = TooltipManager.Instance.textFieldDelimiter;
                parameterizedTextFields.Add(new ParameterizedTextField()
                    { name=parameterName, placeholder = String.Format("{0}{1}{0}", delimiter, parameterName), value = text });
            }
        }

        public void SetImage(string parameterName, Sprite sprite)
        {
            // If the list of dynamic image fields doesn't exist, create it.
            if (dynamicImageFields == null)
                dynamicImageFields = new List<DynamicImageField>();

            // Check to see if we find a matching field. If so, set its sprite to what was passed in.
            bool fieldExists = false;
            for (int i = 0; i < dynamicImageFields.Count; i++)
            {
                if (dynamicImageFields[i].name == parameterName)
                {
                    dynamicImageFields[i].replacementSprite = sprite;
                    fieldExists = true;
                }
            }

            // Finally, if the image field doesn't exist in the list, create it and set its image to what was passed in.
            if (!fieldExists)
            {
                dynamicImageFields.Add(new DynamicImageField() 
                    { name = parameterName, placeholderSprite = null, replacementSprite = sprite });
            }
        }

        public void TurnSectionOn(string parameterName)
        {
            ToggleSection(parameterName, true);
        }

        public void TurnSectionOff(string parameterName)
        {
            ToggleSection(parameterName, false);
        }

        public void ToggleSection(string parameterName, bool isOn)
        {
            // If the list of dynamic section fields doesn't exist, create it.
            if (dynamicSectionFields == null)
                dynamicSectionFields = new List<DynamicSectionField>();

            // Check to see if we find a matching field. If so, set its sprite to what was passed in.
            bool fieldExists = false;
            for (int i = 0; i < dynamicSectionFields.Count; i++)
            {
                if (dynamicSectionFields[i].name == parameterName)
                {
                    dynamicSectionFields[i].isOn = isOn;
                    fieldExists = true;
                }
            }

            // Finally, if the image field doesn't exist in the list, create it and set its image to what was passed in.
            if (!fieldExists)
                dynamicSectionFields.Add(new DynamicSectionField() {name = parameterName, isOn = isOn});
        }
    }
}
