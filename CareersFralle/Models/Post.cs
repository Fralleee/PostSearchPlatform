using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareersFralle.Models;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Host { get; set; }
    public string? Url { get; set; }
    public Post() { }

    public Post(string title, string description, string host, string url)
    {
        Title = title;
        Description = description;
        Host = host;
        Url = url;
    }

    public string CreatedDateFormatted => Created.ToShortDateString();

}
