using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.UtilsModels.PathsEnum;

namespace Models.BussinessModels
{
    public class Path
    {
        public ePathType Type { get; set; } = ePathType.byCost;
        public ePathStatus Status { get; set; } = ePathStatus.foundBestPath;
        public Node StartNode { get; set; } = new Node();
        public Node EndNode { get; set; } = new Node();
        public List<Node> NodesTaken { get; set; } = new List<Node>();
        public List<Connection> ConnectionsTaken { get; set; } = new List<Connection>();
        public decimal TotalTime { get; set; } = 0;
        public decimal TotalCost { get; set; } = 0;

        public override bool Equals(object obj)
        {
            if (!(obj is Path)) return false;

            Path model = obj as Path;

            if (this.Type != model.Type) return false;
            if (this.Status != model.Status) return false;
            if (!this.StartNode.Equals(model.StartNode)) return false;
            if (!this.EndNode.Equals(model.EndNode)) return false;

            if (this.NodesTaken.Count != model.NodesTaken.Count) return false;
            for (int i = 0; i < NodesTaken.Count; i++)
                if (!this.NodesTaken[i].Equals(model.NodesTaken[i])) return false;

            if (this.ConnectionsTaken.Count != model.ConnectionsTaken.Count) return false;
            for (int i = 0; i < ConnectionsTaken.Count; i++)
                if (!this.ConnectionsTaken[i].Equals(model.ConnectionsTaken[i])) return false;

            if (TotalCost != model.TotalCost) return false;
            if (TotalTime != model.TotalTime) return false;

            return true;
        }
    }

   




}
