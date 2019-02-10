using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.UtilsModels
{
    public class PathsEnum
    {
        public enum ePathStatus
        {
            [Description( "Found Best Path")]
            foundBestPath = 0,
            [Description( "Invalid Nodes Given")]
            invalidNodesGiven = 1,
            [Description("Not Connected Nodes Given")]
            notConnectedNodesGiven = 2,
            [Description( "Found Only Immediate Path")]
            foundOnlyImmediatePath = 3,
            [Description( "Oops Something Went Wrong")]
            WebServiceError = 4
        }

        public enum ePathType
        {
            [Description( "Get best path by Time")]
            byTime = 0,
            [Description( "Get best path by Cost")]
            byCost = 1,
        }
    }
}
