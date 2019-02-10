using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.UtilsModels
{
    public class Filter
    {
        public Node StartNode = new Node(); 
        public Node EndNode = new Node();
       
        public bool Validate()
        {
            return true;
        }
    }

}
