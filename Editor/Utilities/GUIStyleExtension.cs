using UnityEngine;

namespace MuffinDev.MultipleEditors.Utilities
{

    /// <summary>
    /// Extensions for GUIStyle class.
    /// </summary>
    public static class GUIStyleExtension
    {

        public const int MIN_FONT_SIZE = 1;
        public const int MAX_FONT_SIZE = 255;

        /// <summary>
        /// Copies the input GUIStyle, and enables/disables word wrapping.
        /// </summary>
        public static GUIStyle WordWrap(this GUIStyle _Style, bool _Enable)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.wordWrap = _Enable;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and enables/disables rich text mode.
        /// </summary>
        public static GUIStyle RichText(this GUIStyle _Style, bool _Enable)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.richText = _Enable;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and enables/disables width stretching.
        /// </summary>
        public static GUIStyle StretchWidth(this GUIStyle _Style, bool _Enable)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.stretchWidth = _Enable;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and enables/disables height stretching.
        /// </summary>
        public static GUIStyle StretchHeight(this GUIStyle _Style, bool _Enable)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.stretchHeight = _Enable;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the given font size.
        /// </summary>
        public static GUIStyle FontSize(this GUIStyle _Style, int _FontSize)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.fontSize = Mathf.Clamp(_FontSize, MIN_FONT_SIZE, MAX_FONT_SIZE);
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the given font color on normal state.
        /// </summary>
        public static GUIStyle FontColor(this GUIStyle _Style, Color _Color)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.normal.textColor = _Color;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the text alignment.
        /// </summary>
        public static GUIStyle TextAlignment(this GUIStyle _Style, TextAnchor _Alignment)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.alignment = _Alignment;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the font style.
        /// </summary>
        public static GUIStyle FontStyle(this GUIStyle _Style, FontStyle _FontStyle)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.fontStyle = _FontStyle;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the margins.
        /// </summary>
        public static GUIStyle Margin(this GUIStyle _Style, RectOffset _Offset)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.margin = _Offset;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the margins.
        /// </summary>
        public static GUIStyle Margin(this GUIStyle _Style, int _HorizontalOffset, int _VerticalOffset)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.margin = new RectOffset(_HorizontalOffset, _HorizontalOffset, _VerticalOffset, _VerticalOffset);
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the margins.
        /// </summary>
        public static GUIStyle Margin(this GUIStyle _Style, int _Left, int _Right, int _Top, int _Bottom)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.margin = new RectOffset(_Left, _Right, _Top, _Bottom);
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the padding.
        /// </summary>
        public static GUIStyle Padding(this GUIStyle _Style, RectOffset _Offset)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.padding = _Offset;
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the padding.
        /// </summary>
        public static GUIStyle Padding(this GUIStyle _Style, int _HorizontalOffset, int _VerticalOffset)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.padding = new RectOffset(_HorizontalOffset, _HorizontalOffset, _VerticalOffset, _VerticalOffset);
            return style;
        }

        /// <summary>
        /// Copies the input GUIStyle, and sets the padding.
        /// </summary>
        public static GUIStyle Padding(this GUIStyle _Style, int _Left, int _Right, int _Top, int _Bottom)
        {
            GUIStyle style = new GUIStyle(_Style);
            style.padding = new RectOffset(_Left, _Right, _Top, _Bottom);
            return style;
        }

    }

}