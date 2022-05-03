using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using APIContagem;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// GitHub do ASP.NET API Versioning:
// https://github.com/microsoft/aspnet-api-versioning

// GitHub do projeto que utilizei como base para a
// a implementacaoo desta aplicacaoo:
// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample

// Algumas referencias sobre ASP.NET API Versioning:
// https://devblogs.microsoft.com/aspnet/open-source-http-api-packages-and-tools/
// https://www.hanselman.com/blog/aspnet-core-restful-web-api-versioning-made-easy

builder.Services.AddApiVersioning(options =>
{
    // Retorna os headers "api-supported-versions" e "api-deprecated-versions"
    // indicando versoes suportadas pela API e o que esta como deprecated
    options.ReportApiVersions = true;

    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    // Agrupar por numero de versao
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // Necessario para o correto funcionamento das rotas
    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Geracaoo de um endpoint do Swagger para cada versao descoberta
    foreach (var description in
        app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();