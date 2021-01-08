namespace Hide.Network
{
    /// <summary>
    /// Modified from BeardedManStudios.Forge.Networking.Lobby
    /// </summary>
    public interface IClientHidePlayer
    {
        uint NetworkId { get; set; }
        string Name { get; set; }
        int AvatarID { get; set; }
        int TeamID { get; set; }
        HidePlayerRole Role { get; set; }
    }

    public enum HidePlayerRole
    {
        Animal,
        Wolf
    }
}