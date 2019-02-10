using System;
using System.Collections.Generic;
using System.Text;

namespace Models.BussinessModels
{
    public class Node
    {
        public int ID { get; set; } = 0;

        public string Name { get; set; } = String.Empty;

        public bool Validate(bool checkID = false)
        {
            if (checkID && ID <= 0) return false;
            if (String.IsNullOrEmpty(Name)) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Node)) return false;

            Node model = obj as Node;

            if (this.ID != model.ID) return false;
            if (this.Name != model.Name) return false;

            return true;
        }


    }
}
