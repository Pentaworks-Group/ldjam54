using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Prefabs.Menues.Book
{
    public class PageBehaviour : MonoBehaviour
    {

        public String indexName;
        private BookBehaviour bookBehaviour;

        public void SetPageBehaviour(BookBehaviour bookBehaviour)
        {
            this.bookBehaviour = bookBehaviour;
        }
        public void OpenThisPage()
        {
            bookBehaviour.OpenPage(this);
        }



    }
}
