using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment.Attributes
{
    public class TagsAttribute
    {
        public List<string> Tags;

        public TagsAttribute(params string[] args)
        {
            Tags = new List<string>();

            foreach (var arg in args)
                Tags.Add(arg);
        }
    }
}
