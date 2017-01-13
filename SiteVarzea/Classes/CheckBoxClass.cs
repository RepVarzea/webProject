using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteVarzea.Classes
{
    public class CheckBoxClass
    {
    }

    public class CollectionVM
    {
        public List<ChoiceViewModel> ChoicesVM { get; set; }
        public List<Int64> SelectedChoices { get; set; }
    }

    public class ChoiceViewModel
    {
        public Int64 SNo { get; set; }
        public string Text { get; set; }
    }
}