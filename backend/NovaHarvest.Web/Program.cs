var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Servir estáticos desde wwwroot (css/site.css e img/logo.png)
app.MapStaticAssets(); 

// Función auxiliar para construir el HTML base común 
string BuildPageHtml(string title, string mainContent)
{
    return $"""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <title>NovaHarvest - {title}</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <img src="/img/logo.png" alt="Logotipo NovaHarvest">
            <span>NovaHarvest</span>
            <nav>
                <a href="/">Inicio</a>
                <a href="/about">Acerca de</a>
                <a href="/status">Estado</a>
            </nav>
        </header>

        <main>
            <h1>{title}</h1>
            {mainContent}
        </main>

        <footer>
            NovaHarvest · IES Valle del Jerte · {DateTime.Now.Year}
        </footer>
    </body>
    </html>
    """;
}

// 1. Ruta / (Inicio)
app.MapGet("/", () => Results.Content(
    BuildPageHtml("Inicio", """
        <p>NovaHarvest es una plataforma diseñada para conectar a la cooperativa agraria del Valle del Jerte y sus productores con posibles compradores como residencias, restaurantes y particulares.</p>
        <p>Su objetivo principal es canalizar los excedentes semanales de fruta y verdura que no llegan a comercializarse por vías habituales.</p>
        <p>Mediante la publicación de disponibilidad y recetas de aprovechamiento, la herramienta evita que alimentos de calidad terminen en la basura.</p>
    """),
    "text/html"
));

// 2. Ruta /about
app.MapGet("/about", () => Results.Content(
    BuildPageHtml("Acerca de", """
        <p><strong>Proyecto:</strong> NovaHarvest (Fase UT2)</p>
        <p><strong>Centro:</strong> IES Valle del Jerte (Plasencia)</p>
        <p><strong>Autor:</strong> Hugo Izquierdo </p>
        <p><strong>Versión:</strong> 0.1.0</p>
    """),
    "text/html"
));

// 3. Ruta /status
app.MapGet("/status", () => Results.Content(
    BuildPageHtml("Estado del servidor", $"""
        <p>Fecha y hora del servidor: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
        <p>Página generada por ASP.NET Core (Kestrel)</p>
    """),
    "text/html"
));

app.Run();