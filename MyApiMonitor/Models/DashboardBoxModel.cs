namespace MyApiMonitor.Models
{
    public class DashboardBoxModel
    {
        /// <summary>
        /// The html to appear in the header of the DashboardSubSection
        /// </summary>
        public string? Header { get; set; }

        /// <summary>
        /// The html content to appear in the middle of the DashboardSubSection.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The html to appear in the footer of the DashboardSubSection.
        /// </summary>
        public string? Footer { get; set; }

        /// <summary>
        /// Additional Classes to be added to Content Section of Dashboard Box
        /// </summary>
        /// <remarks>
        /// 'big-number' = will format the text content in a big centralised bold font.
        /// </remarks>
        public string? ContentClasses { get; set; }

        public DashboardBoxModel(string? header, string? content, string? footer, string? contentClasses = null)
        {
            Header = header;
            Content = content;
            Footer = footer;
            ContentClasses = contentClasses;
        }

    }
}
