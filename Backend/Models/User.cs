using System.ComponentModel.DataAnnotations;


public class User
{
	public int UserId { get; set; }
	[Required]
	public string Username { get; set; }
	[Required]
	public string Email { get; set; }
	public string PasswordHash { get; set; }
}
