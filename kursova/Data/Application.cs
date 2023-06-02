using kursova.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace kursova.Data;

public partial class Application
{
    public int Id { get; set; }

    public DateTime? Date { get; set; } = DateTime.Today;
    public string Content { get; set; } = null!;

    public string? Status { get; set; }

    public short? QueueNum { get; set; }

    public int CitizenId { get; set; }

    public int? SpecialistId { get; set; }

    public virtual Citizen? Citizen { get; set; } = null!;

    public virtual Specialist? Specialist { get; set; }

    public void FillApplication(UpdateApplicationViewModel updateApplicationViewModel, KursovaContext _kursovaContext)
    {
        Id = updateApplicationViewModel.Id;
        Date = updateApplicationViewModel.Date;
        Content = updateApplicationViewModel.Content;
        Status = updateApplicationViewModel.Status;
        CitizenId = updateApplicationViewModel.CitizenId;
        SpecialistId = updateApplicationViewModel.SpecialistId;
        Citizen = _kursovaContext.Citizens.FirstOrDefault(
                        x => x.Id == updateApplicationViewModel.CitizenId);
        Specialist = _kursovaContext.Specialists.FirstOrDefault(
                x => x.Id == updateApplicationViewModel.SpecialistId);
    }
}
