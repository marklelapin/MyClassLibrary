namespace MyClassLibrary.ChartJs
{
    public class Functions
    {
        public string CallBackPattern = "\"callbackfunction.*?.\"";

        public string JsFunctionPattern = "\"jsfunction.*?.\"";

        public string GetJavascriptFunction(string functionProperty)
        {
            return GetFunctionString(functionProperty, "\"jsfunction.");
        }

        public string GetCallBackFunction(string callbackProperty)
        {
            try
            {
                string callbackString = GetFunctionString(callbackProperty, "\"callbackfunction.");

                string callback = string.Empty;

                if (callbackString.Contains("("))
                {

                    var callbackFunction = callbackString.Substring(0, callbackString.IndexOf("("));
                    var callbackParameter = callbackString.Replace(callbackFunction, "")
                                                         .Replace("(", "")
                                                         .Replace(")", "");

                    callback = CallbackFunctions[callbackFunction].Replace(".dynamicParameter.", callbackParameter);

                }
                else
                {
                    callback = CallbackFunctions[callbackString];
                }

                return callback;
            }
            catch
            {
                return "not found";
            };
        }
        /// <summary>
        /// Dictionary of call back function that can be used in My Chart.Js.
        /// </summary>
        /// <remarks>
        /// 'ConvertTickToDateTime' requires moment.js to be added to scripts.
        /// </remarks>
        private Dictionary<string, string> CallbackFunctions = new Dictionary<string, string>()
        {
            { "UseTickLabels", $@"function(value,index,ticks) {{
                                            switch(value) {{
                                                    .dynamicParameter.                                                       
                                                    default: return '';
                                                }}
                                            }}" }
            ,{ "ConvertTickToDateTime", $@"function(value) {{
                                                
                                               
                                                return moment(value).format(.dynamicParameter.);

                                            }}"

            }
            ,{"ConvertLabelToDateTime", $@"function(value){{
                                                var label = this.getLabelForValue(value)

                                                return moment(label).format(.dynamicParameter.);
                                            }}"}
            ,{"OverrideTickValues",$@"axis => {{
                                          axis.ticks = .dynamicParameter.
                                        }}"}
        };



        private string GetFunctionString(string functionProperty, string preFix)
        {
            return functionProperty.Replace(preFix, "").Replace(".\"", "");
        }





    }
}
