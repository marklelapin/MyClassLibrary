namespace MyClassLibrary.ChartJs
{
    public class CallBackFunctionLibrary
    {
        public string CallBackPattern = "\"callbackfunction.*?.\"";


        public string GetCallBackFunction(string callbackProperty)
        {
            try
            {
                string callbackString = callbackProperty.Replace("\"callbackfunction.", "")
                                                        .Replace(".\"", "");


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

        private Dictionary<string, string> CallbackFunctions = new Dictionary<string, string>()
        {
            {"UseTickLabels", $@"function(value,index,ticks) {{
                                            switch(value) {{
                                                    .dynamicParameter.                                                       
                                                    default: return '';
                                                }}
                                            }}" }
        };
    }
}
