namespace Hackathon_2024_INFISOFTWARE.Domain.Configs
{
    /// <summary>
    /// Getting data to use in security Emmail
    /// </summary>
    public class EmailOption
    {
        #region Properties
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        #endregion
    }
}
