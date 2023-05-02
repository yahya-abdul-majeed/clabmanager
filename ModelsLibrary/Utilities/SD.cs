namespace ModelsLibrary.Utilities
{
    public enum GridType
    {
        None = 0,
        type1 = 1,
        type2 = 2
    }
    public enum LabStatus
    {
        Occupied,
        Vacant,
        Maintenance
    }
    public enum IssuePriority
    {
        Urgent,
        Normal,
        Low
    }

    public enum IssueState
    {
        Unhandled,
        Handled,
        InProcess
    }
    public static class SD
    {
        public const string XAccessToken = "X-Access-Token";

       

    }
}
