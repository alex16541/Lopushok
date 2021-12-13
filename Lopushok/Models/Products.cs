using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lopushok.Models
{
    partial class Product
    {
        public string ValidImage => String.IsNullOrEmpty(Image) ? "/img/products/picture.png" : Image;
        public string ValidMaterials
        {
            get
            {
                string result = "";
                List<Material> materials = Material.ToList();

                for (int i = 0; i < materials.Count; i++)
                {
                    if(i == materials.Count - 1)
                    {
                        result += materials[i].Title + ".";
                    }
                    else
                    {
                        result += materials[i].Title + ", ";
                    }
                }

                return result;
            }
        }
    }
}
