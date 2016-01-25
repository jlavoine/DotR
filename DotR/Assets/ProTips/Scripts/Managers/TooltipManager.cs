using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ModelShark
{
    /// <summary>Singleton game object that manages showing, hiding, and resizing tooltips.</summary>
    /// <remarks>Put this script on a game object in your scene (but only once) in order to use ProTips.</remarks>
    public class TooltipManager : MonoBehaviour
    {
        [Tooltip("When enabled, tooltips will be triggered by pressing-and-holding, not hovering over. They will be dismissed by releasing the hold, instead of hover off.")]
        public bool touchSupport = false;
        public float tooltipDelay = 0.33f;
        public float fadeDuration = 0.2f;
        public string textFieldDelimiter = "%";

        private static TooltipManager instance;
        private bool isInitialized;

        public static TooltipManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<TooltipManager>();
                if (!instance.isInitialized)
                    instance.Initialize();
                return instance;
            }
        }

        /// <summary>The root parent canvas component.</summary>
        public Canvas RootCanvas { get; set; }

        /// <summary>Holds a reference to the in-scene, runtime gameobject associated with each tooltip style prefab.</summary>
        public Dictionary<TooltipStyle, Tooltip> Tooltips { get; set; }

        /// <summary>When populated, this is the only tooltip allowed on the screen.</summary>
        public Tooltip BlockingTooltip { get; set; }

        private void Awake()
        {
            instance = this;
            if (!isInitialized)
                Initialize();
        }

        private void Initialize()
        {
            if (isInitialized) return;

            RootCanvas = CanvasHelper.GetRootCanvas();
            Tooltips = new Dictionary<TooltipStyle, Tooltip>();
            isInitialized = true;
        }

        /// <summary>Sets the parameterized text fields on the tooltip.</summary>
        /// <remarks>This is separate from the Show() method because we need to wait a frame or two so the text's RectTransform has time to update its preferredWidth.</remarks>
        public void SetTextAndSize(TooltipTrigger trigger)
        {
            if (trigger.Tooltip == null || trigger.parameterizedTextFields == null) return;

            if (trigger.Tooltip.TextFields == null || trigger.Tooltip.TextFields.Count == 0) return;

            LayoutElement mainTextContainer = trigger.Tooltip.TooltipStyle.mainTextContainer;
            if (mainTextContainer == null)
                Debug.LogWarning(String.Format("No main text container defined on tooltip style \"{0}\". Note: This LayoutElement is needed in order to resize text appropriately.", trigger.Tooltip.GameObject.name));
            else
                mainTextContainer.preferredWidth = trigger.minTextWidth;

            for (int i = 0; i < trigger.Tooltip.TextFields.Count; i++)
            {
                Text txt = trigger.Tooltip.TextFields[i].Text;
                if (txt.text.Length < 3) continue; // text is too short to contain a parameter, so skip it.
                
                for (int j = 0; j < trigger.parameterizedTextFields.Count; j++)
                {
                    if (!String.IsNullOrEmpty(trigger.parameterizedTextFields[j].value))
                        txt.text = txt.text.Replace(trigger.parameterizedTextFields[j].placeholder, trigger.parameterizedTextFields[j].value);
                }
                 
                if (mainTextContainer != null)
                {
                    // if the text would be wider than allowed, constrain the main text container to that limit.
                    if (txt.preferredWidth > trigger.maxTextWidth) 
                        mainTextContainer.preferredWidth = trigger.maxTextWidth;
                    // otherwise, if it's within the allotted space but bigger than the text container's default width, expand the main text container to accommodate.
                    else if (txt.preferredWidth > trigger.minTextWidth && txt.preferredWidth > mainTextContainer.preferredWidth)
                        mainTextContainer.preferredWidth = txt.preferredWidth;
                }
            }
        }
         
        /// <summary>Displays the tooltip.</summary> 
        /// <remarks>
        /// This method first waits a couple frames before sizing and positioning the tooltip. 
        /// This is necessary in order to get an accurate preferredWidth property of the dynamic text field.
        /// </remarks>
        public IEnumerator Show(TooltipTrigger trigger)
        {
            if (trigger.tooltipStyle == null)
            {
                Debug.LogWarning("TooltipTrigger \"" + trigger.name + "\" has no associated TooltipStyle. Cannot show tooltip.");
                yield break;
            }

            Image tooltipBkgImg = trigger.Tooltip.BackgroundImage;

            // Replace dynamic image placeholders with the correct images.
            if (trigger.dynamicImageFields != null)
            {
                for (int i = 0; i < trigger.dynamicImageFields.Count; i++)
                {
                    for (int j = 0; j < trigger.Tooltip.ImageFields.Count; j++)
                    {
                        if (trigger.Tooltip.ImageFields[j].Name == trigger.dynamicImageFields[i].name)
                        {
                            if (trigger.dynamicImageFields[i].replacementSprite == null)
                                trigger.Tooltip.ImageFields[j].Image.sprite = trigger.Tooltip.ImageFields[j].Original;
                            else
                                trigger.Tooltip.ImageFields[j].Image.sprite = trigger.dynamicImageFields[i].replacementSprite;
                        }
                    }
                }
            }

            // Toggle dynamic sections on or off.
            if (trigger.dynamicSectionFields != null)
            {
                for (int i = 0; i < trigger.dynamicSectionFields.Count; i++)
                {
                    for (int j = 0; j < trigger.Tooltip.SectionFields.Count; j++)
                    {
                        if (trigger.Tooltip.SectionFields[j].Name == trigger.dynamicSectionFields[i].name)
                            trigger.Tooltip.SectionFields[j].GameObject.SetActive(trigger.dynamicSectionFields[i].isOn);
                    }
                }
            }

            // Wait for 2 frames so we get an accurate PreferredWidth on the Text component.
            yield return WaitFor.Frames(2);

            // SET POSITION OF TOOLTIP
            bool useMousePosition = trigger.tipPosition == TipPosition.MouseBottomLeftCorner || trigger.tipPosition == TipPosition.MouseTopLeftCorner || 
                                    trigger.tipPosition == TipPosition.MouseBottomRightCorner || trigger.tipPosition == TipPosition.MouseTopRightCorner;
            Vector3[] triggerCorners = new Vector3[4];
            trigger.RectTransform.GetWorldCorners(triggerCorners);
            // Corners:
            // 0 = bottom left
            // 1 = top left
            // 2 = top right
            // 3 = bottom right
            RectTransform tooltipRT = trigger.Tooltip.RectTransform;
            SetTooltipPosition(trigger.tipPosition, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
            
            // CHECK FOR OVERFLOW: Reposition tool tip if outside root canvas rect.
            Rect screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] tooltipCorners = new Vector3[4];
            trigger.Tooltip.RectTransform.GetWorldCorners(tooltipCorners);
            bool isOverBottomLeft = !screenRect.Contains(tooltipCorners[0]);
            bool isOverTopLeft = !screenRect.Contains(tooltipCorners[1]);
            bool isOverTopRight = !screenRect.Contains(tooltipCorners[2]);
            bool isOverBottomRight = !screenRect.Contains(tooltipCorners[3]);

            switch (trigger.tipPosition)
            {
                case TipPosition.TopRightCorner:
                case TipPosition.MouseTopRightCorner:
                    if (isOverTopLeft && isOverTopRight && isOverBottomRight) // if top and right edges are off the canvas, flip tooltip position to opposite corner.
                        SetTooltipPosition(TipPosition.BottomLeftCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverTopLeft && isOverTopRight) // if only the top edge is off the canvas, flip tooltip position to bottom.
                        SetTooltipPosition(TipPosition.BottomRightCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverTopRight && isOverBottomRight) // if only the right edge is off the canvas, flip tooltip position to left.
                        SetTooltipPosition(TipPosition.TopLeftCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    break;
                case TipPosition.BottomRightCorner:
                case TipPosition.MouseBottomRightCorner:
                    if (isOverBottomLeft && isOverBottomRight && isOverTopRight) // if bottom and right edges are off the canvas, flip tooltip position to opposite corner.
                        SetTooltipPosition(TipPosition.TopLeftCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverBottomLeft && isOverBottomRight) // if only the bottom edge is off the canvas, flip tooltip position to top.
                        SetTooltipPosition(TipPosition.TopRightCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverTopRight && isOverBottomRight) // if only the right edge is off the canvas, flip tooltip position to left.
                        SetTooltipPosition(TipPosition.BottomLeftCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    break;
                case TipPosition.TopLeftCorner:
                case TipPosition.MouseTopLeftCorner:
                    if (isOverTopLeft && isOverTopRight && isOverBottomLeft) // if top and left edges are off the canvas, flip tooltip position to opposite corner.
                        SetTooltipPosition(TipPosition.BottomRightCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverTopLeft && isOverTopRight) // if only the top edge is off the canvas, flip tooltip position to bottom.
                        SetTooltipPosition(TipPosition.BottomLeftCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverTopLeft && isOverBottomLeft) // if only the left edge is off the canvas, flip tooltip position to right.
                        SetTooltipPosition(TipPosition.TopRightCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    break;
                case TipPosition.BottomLeftCorner:
                case TipPosition.MouseBottomLeftCorner:
                    if (isOverBottomLeft && isOverBottomRight && isOverTopLeft) // if bottom and left edges are off the canvas, flip tooltip position to opposite corner.
                        SetTooltipPosition(TipPosition.TopRightCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverBottomLeft && isOverBottomRight) // if only the bottom edge is off the canvas, flip tooltip position to top.
                        SetTooltipPosition(TipPosition.TopLeftCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    else if (isOverTopLeft && isOverBottomLeft) // if only the left edge is off the canvas, flip tooltip position to right.
                        SetTooltipPosition(TipPosition.BottomRightCorner, trigger.Tooltip.GameObject, trigger.tooltipStyle, triggerCorners, tooltipBkgImg, tooltipRT, useMousePosition);
                    break;
            }
            
            // Set the tint color of the tooltip panel and tips.
            tooltipBkgImg.color = trigger.backgroundTint;

            // Fade the tooltip in.
            trigger.Tooltip.FadeIn(fadeDuration);
        }

        private static void SetTooltipPosition(TipPosition tipPosition, GameObject tooltipObj, TooltipStyle style, Vector3[] triggerCorners, Image tooltipImage, RectTransform tooltipRectTrans, bool useMousePosition)
        {
            Vector3 pos = Vector3.zero;
            Vector3 offsetVector;
            switch (tipPosition)
            {
                case TipPosition.TopRightCorner:
                case TipPosition.MouseTopRightCorner:
                    offsetVector = new Vector3(-1 * style.tipOffset, -1 * style.tipOffset);
                    if (useMousePosition)
                        pos = Input.mousePosition + offsetVector;
                    else
                        pos = triggerCorners[2] + offsetVector;
                    tooltipRectTrans.pivot = new Vector2(0, 0);
                    tooltipImage.sprite = style.bottomLeftCorner;
                    break;
                case TipPosition.BottomRightCorner:
                case TipPosition.MouseBottomRightCorner:
                    offsetVector = new Vector3(-1 * style.tipOffset, style.tipOffset);
                    if (useMousePosition)
                        pos = Input.mousePosition + offsetVector;
                    else
                        pos = triggerCorners[3] + offsetVector;
                    tooltipRectTrans.pivot = new Vector2(0, 1);
                    tooltipImage.sprite = style.topLeftCorner;
                    break;
                case TipPosition.TopLeftCorner:
                case TipPosition.MouseTopLeftCorner:
                    offsetVector = new Vector3(style.tipOffset, -1 * style.tipOffset);
                    if (useMousePosition)
                        pos = Input.mousePosition + offsetVector;
                    else
                        pos = triggerCorners[1] + offsetVector;
                    tooltipRectTrans.pivot = new Vector2(1, 0);
                    tooltipImage.sprite = style.bottomRightCorner;
                    break;
                case TipPosition.BottomLeftCorner:
                case TipPosition.MouseBottomLeftCorner:
                    offsetVector = new Vector3(style.tipOffset, style.tipOffset);
                    if (useMousePosition)
                        pos = Input.mousePosition + offsetVector;
                    else
                        pos = triggerCorners[0] + offsetVector;
                    tooltipRectTrans.pivot = new Vector2(1, 1);
                    tooltipImage.sprite = style.topRightCorner;
                    break;
            }
            // Set the position of the tooltip container.
            tooltipObj.transform.position = pos;
        }

        /// <summary>Closes all visible tooltips.</summary>
        /// <remarks>Useful for when your player may have a tutorial tooltip up and hits ESC to go to the main menu, for example.</remarks>
        public void CloseAll()
        {
            // NOTE: If you needed to, you could store a reference to the open tooltip in the TooltipManager so you could re-open it later, 
            // when the player picks up where they left off.
            TooltipTrigger[] tooltipTriggers = FindObjectsOfType<TooltipTrigger>();
            for (int i = 0; i < tooltipTriggers.Length; i++)
                tooltipTriggers[i].ForceHideTooltip();
        }
    }
}