using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs
{
  public class PlatformPublishedDto
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Event { get; set; }
  }
}