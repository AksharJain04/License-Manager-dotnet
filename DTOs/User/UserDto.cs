public class UserDto {
    public string? ID {get; set;}
    public string? Username {get; set;}
    public string? Email {get; set;}
    public IList<string> Roles {get; set;} = new List<string>();
}