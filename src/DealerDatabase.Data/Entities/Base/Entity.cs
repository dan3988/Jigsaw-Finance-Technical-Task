using System.ComponentModel.DataAnnotations;

namespace DealerDatabase.Data.Entities.Base;

public abstract record Entity
{
	[Key]
	public int Id { get; set; }
}