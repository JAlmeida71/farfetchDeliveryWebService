using System;
using System.Collections.Generic;
using System.Text;

namespace Models.BussinessModels
{
    public class Connection
    {
        public int ID { get; set; }
        public Node StartNode { get; set; } = new Node();
        public Node EndNode { get; set; } = new Node();
        public decimal Cost { get; set; } = 0;
        public decimal Time { get; set; } = 0;

        public bool Validate(bool checkID = false)
        {
            if (checkID && ID <= 0) return false;
            if (this.StartNode.ID <= 0) return false;
            if (this.EndNode.ID <= 0) return false;
            if (this.Cost <= 0) return false;
            if (this.Time <= 0) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Connection)) return false;

            Connection model = obj as Connection;

            if (this.StartNode.ID != model.StartNode.ID) return false;
            if (this.EndNode.ID != model.EndNode.ID) return false;
            if (this.Cost != model.Cost) return false;
            if (this.Time != model.Time) return false;

            return true;
        }

    }
}
