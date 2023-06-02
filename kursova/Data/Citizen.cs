using System;
using System.Collections.Generic;

namespace kursova.Data;

public partial class Citizen
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public override string ToString()
    {
        return $"{Id} {FirstName} {LastName}";
    }
}
