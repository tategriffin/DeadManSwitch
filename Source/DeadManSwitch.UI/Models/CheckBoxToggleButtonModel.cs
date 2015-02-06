using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadManSwitch.UI.Models
{
    public class CheckBoxToggleButtonModel
    {
        //These also exist in shared.js
        private const string CheckBoxIdSuffix = "";
        private const string ToggleButtonIdSuffix = "ToggleBtn";

        public CheckBoxToggleButtonModel(bool value, string id, string text)
        {
            HtmlId = id;
            LabelText = text;
            Value = value;
        }

        public string HtmlId { get; set; }
        public string LabelText { get; set; }
        public bool Value { get; set; }
        //onoffswitch depends on "checked" rather than value="true"
        public string CheckedHtml { get { return (Value ? "checked" : string.Empty); } }
        public string CheckBoxId { get { return HtmlId + CheckBoxIdSuffix; } }
        public string ToggleButtonId { get { return HtmlId + ToggleButtonIdSuffix; } }

    }
}