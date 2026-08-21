using Gerald.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<GeraldDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<VendorRepository>();
builder.Services.AddScoped<IAuditRiskAssessmentService, AuditRiskAssessmentService>();
builder.Services.AddScoped<ILanguageModelClient, StubLanguageModelClient>();
builder.Services.AddScoped<IEmbeddingClient, StubEmbeddingClient>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize database with seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GeraldDbContext>();
    db.Database.EnsureCreated();
    SeedData(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseRouting();
app.MapControllers();

app.Run();

void SeedData(GeraldDbContext db)
{
    if (db.Vendors.Any()) return;

    var vendors = new[]
    {
        new { Name = "Acme Corp", RegNumber = "ACM-001", Status = "Active" },
        new { Name = "BuildCo", RegNumber = "BLD-002", Status = "Active" },
        new { Name = "DataSys", RegNumber = "DAT-003", Status = "Active" },
        new { Name = "ProServices", RegNumber = "PRO-004", Status = "Under Review" },
        new { Name = "TechVend", RegNumber = "TEC-005", Status = "Active" }
    };

    foreach (var v in vendors)
    {
        var vendor = new Gerald.Core.Entities.Vendor
        {
            Name = v.Name,
            RegistrationNumber = v.RegNumber,
            RegistrationDate = DateTime.Now.AddMonths(-6),
            Status = v.Status
        };
        db.Vendors.Add(vendor);
    }

    db.SaveChanges();

    var findings = new[]
    {
        (VendorId: 1, Severity: "High", Notes: "Missing compliance certification"),
        (VendorId: 1, Severity: "Medium", Notes: "Documentation outdated"),
        (VendorId: 1, Severity: "Low", Notes: "Minor process deviation"),
        (VendorId: 2, Severity: "Medium", Notes: "Late delivery on Q3 shipment"),
        (VendorId: 2, Severity: "Low", Notes: "Invoice format inconsistency"),
        (VendorId: 3, Severity: "High", Notes: "Security audit failed"),
        (VendorId: 3, Severity: "High", Notes: "Data breach incident reported"),
        (VendorId: 4, Severity: "Medium", Notes: "Quality issues detected in batch"),
        (VendorId: 4, Severity: "Medium", Notes: "Staffing concerns raised"),
        (VendorId: 5, Severity: "Low", Notes: "Minor labeling issue"),
        (VendorId: 5, Severity: "Low", Notes: "Warehouse organization needs improvement"),
        (VendorId: 2, Severity: "High", Notes: "Environmental violation notice")
    };

    foreach (var f in findings)
    {
        db.AuditFindings.Add(new Gerald.Core.Entities.AuditFinding
        {
            VendorId = f.VendorId,
            Severity = f.Severity,
            Status = f.Severity == "High" ? "Open" : "Closed",
            Notes = f.Notes,
            FindingDate = DateTime.Now.AddDays(-30)
        });
    }

    db.SaveChanges();
}