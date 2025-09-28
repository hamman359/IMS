using Carter;

using IMS.SharedKernal.Configuration;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCarter(new DependencyContextAssemblyCatalog(
//            IMS.ItemInventory.Api.AssemblyReference.Assembly,
//            IMS.SharedKernal.AssemblyReference.Assembly));

//builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddOpenApi();

// InstallServices is used to register configuration settings that are located in other
// files. More information about how this works and why it is benificial can be found in
// /Configuration/DependencyInjecttion.cs
builder.Services
    .InstallServices(
        builder.Configuration,
        IMS.SharedKernal.AssemblyReference.Assembly,
        IMS.ItemInventory.Api.AssemblyReference.Assembly
        );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Configure the OpenApi endpoint
    app.MapOpenApi();

    // Configure the UI for API documentation and interactions.
    // More information at https://guides.scalar.com/scalar/introduction
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("IMS.ItemInventory API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// Used to automatically register Minimal API endpoints that are located in other files.
// This helps keep Program.cs short and clean and allows for easy introduction of new
// endpoints simply by adding new files. This means you do not have to edit any existing
// files when adding endpoints.
// Additional information on the Carter library can be found at https://github.com/CarterCommunity/Carter
app.MapCarter();

app.UseHttpsRedirection();

await app.RunAsync();