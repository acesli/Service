namespace TechArt.Common.DataContracts.General
{
    public class DC_Result
    {
        public bool Success { get; set; }

        public DC_ErrorCode ErrorCode { get; set; }

        public string Message { get; set; }
    }
}