namespace MyClassLibrary.Pagination
{
    public class PaginationBuilder
    {

        private string? First;
        private string? Previous;
        private string? Last;
        private string? Next;
        private string? AriaLabel;
        private int MiddleTotal = 3;

        private string Href;

        private string Html = "";

        public PaginationBuilder(string href)
        {
            this.Href = href;
        }

        public PaginationBuilder AddFirst(string str) { this.First = str; return this; }

        public PaginationBuilder AddLast(string str) { this.Last = str; return this; }


        public PaginationBuilder AddPrevious(string str) { this.Previous = str; return this; }

        public PaginationBuilder AddNext(string str) { this.Next = str; return this; }

        public PaginationBuilder SetMiddleTotal(int middleTotal) { this.MiddleTotal = middleTotal; return this; }

        public PaginationBuilder AddAriaLabel(string str) { this.AriaLabel = str; return this; }


        public string BuildHtml(int currentPage, int TotalPages)
        {

            if (AriaLabel != null) { Html += $@"<nav aria-label=""{AriaLabel}"">"; }


            Html += @"<ul class = ""pagination"">";

            if (First != null) { AddListLine(First, 1, false, (currentPage == 1)); }

            if (Previous != null) { AddListLine(Previous, currentPage - 1, false, (currentPage == 1)); }

            for (int i = 1; i <= TotalPages; i++)
            {
                if (i >= Math.Min(currentPage - (MiddleTotal / 2), TotalPages - MiddleTotal) && i <= Math.Max(currentPage + (MiddleTotal / 2), MiddleTotal))
                {
                    AddListLine(i.ToString(), i, (i == currentPage), false);
                }
            }

            if (Next != null) { AddListLine(Next, currentPage + 1, false, (currentPage == TotalPages)); }

            if (Last != null) { AddListLine(Last, TotalPages, false, (currentPage == TotalPages)); }

            Html += "</ul>";

            if (AriaLabel != null) { Html += $@"</nav>"; }

            return Html;

        }


        private void AddListLine(string text, int pg, bool isActive, bool isDisabled = false)
        {
            var disabledAddOn = isDisabled ? " disabled" : "";

            var tabIndex = isDisabled ? @"tabindex=""-1""" : "";

            Html += $@"<li class=""page-item{disabledAddOn}"">
                        <a class = ""page-link"" href = ""{Href!.Replace("<page>", pg.ToString())}"" {tabIndex}>{text}</a>
                    </li>";
        }

    }
}
